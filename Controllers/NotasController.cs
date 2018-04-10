using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Controllers {
    [Authorize]
    [Route ("[controller]/{UserId}/{MateriaId}")]
    public class NotasController : Controller {
        private readonly IEstudiante _estudiante;

        public NotasController (IEstudiante _estudiante) => this._estudiante = _estudiante;
        [HttpGet]
        public Task<IActionResult> Get (string UserId, string MateriaId) => GET (UserId, MateriaId);
        private async Task<IActionResult> GET (string UserId, string MateriaId) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                } else {
                    foreach (Semestre sem in es.Semestres) {
                        foreach (Materia mat in sem.Materias) {
                            if (mat.Id == MateriaId) {
                                return Ok (JsonConvert.SerializeObject (mat.Cortes_Notas));
                            }
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable, "Materia No Encontrada");
                }
            } catch (Exception e) {
                return BadRequest (e);
            }
        }

        [HttpGet ("{Corte}")]
        public Task<IActionResult> Get (string UserId, string MateriaId, int Corte) => GET (UserId, MateriaId, Corte);
        private async Task<IActionResult> GET (string UserId, string MateriaId, int Corte) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                } else {
                    foreach (Semestre sem in es.Semestres) {
                        foreach (Materia mat in sem.Materias) {
                            if (mat.Id == MateriaId) {
                                if (Corte > mat.Cortes_Notas.Length || Corte <= 0) {
                                    return StatusCode (StatusCodes.Status406NotAcceptable, "Corte No Encontrado");
                                }
                                return Ok (JsonConvert.SerializeObject (mat.Cortes_Notas[Corte - 1]));
                            }
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable, "Materia No Encontrada");
                }
            } catch (Exception  e) {
                return BadRequest (e);
            }
        }

        [HttpPost ("{Semestre}/{Corte}")]
        public async Task<IActionResult> NotaPost ([FromBody] double nota, string UserId, int Semestre, string MateriaId, int Corte) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable, ModelState);
                } else {
                    if(nota < 0) return StatusCode (StatusCodes.Status406NotAcceptable, "Notas Mayor O Igual A 0");
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null)  return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            foreach (Materia mat in us.Materias) {
                                if (mat.Id == MateriaId) {
                                    if (Corte > mat.Cortes_Notas.Length || Corte <= 0) {
                                        return StatusCode (StatusCodes.Status406NotAcceptable, "Corte No Encontrado");
                                    }
                                    mat.Cortes_Notas[Corte - 1].Add (nota);
                                    var h = await _estudiante.Update (UserId, es);
                                    if (h.MatchedCount > 0) {
                                        return Ok ("Creado");
                                    } else {
                                        return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                    }
                                }
                            }
                            return StatusCode (StatusCodes.Status406NotAcceptable, "Materia No Encontrada");
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable, "No Existe Este Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ah Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpPut ("{Semestre}/{Corte}/{Index}")]
        public async Task<IActionResult> NotaPut ([FromBody] double nota, string UserId, int Semestre, string MateriaId, int Corte, int Index) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable, ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            foreach (Materia mat in us.Materias) {
                                if (mat.Id == MateriaId) {
                                    if (Corte > mat.Cortes_Notas.Length || Corte <= 0) {
                                        return StatusCode (StatusCodes.Status406NotAcceptable, "Corte No Encontrado");
                                    }
                                    for (int i = 0; i < mat.Cortes_Notas[Corte - 1].Count; i++) {
                                        if (i == Index) {
                                            mat.Cortes_Notas[Corte - 1][Index] = nota;
                                            var h = await _estudiante.Update (UserId, es);
                                            if (h.MatchedCount > 0) {
                                                return Ok ("Modificado");
                                            } else {
                                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                            }
                                        }
                                    }
                                    return StatusCode (StatusCodes.Status406NotAcceptable, "Index No Encontrado");
                                }
                            }
                            return StatusCode (StatusCodes.Status406NotAcceptable, "Materia No Encontrada");
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable, "No Existe Este Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ah Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpDelete ("{Semestre}/{Corte}/{Index}")]
        public async Task<IActionResult> NotaDelete (string UserId, int Semestre, string MateriaId, int Corte, int Index) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable, ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            foreach (Materia mat in us.Materias) {
                                if (mat.Id == MateriaId) {
                                    if (Corte > mat.Cortes_Notas.Length || Corte <= 0) {
                                        return StatusCode (StatusCodes.Status406NotAcceptable, "Corte No Encontrado");
                                    }
                                    for (int i = 0; i < mat.Cortes_Notas[Corte - 1].Count; i++) {
                                        if (i == Index) {
                                            mat.Cortes_Notas[Corte - 1].RemoveAt (Index);
                                            var h = await _estudiante.Update (UserId, es);
                                            if (h.MatchedCount > 0) {
                                                return Ok ("Eliminado");
                                            } else {
                                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                            }
                                        }
                                    }
                                    return StatusCode (StatusCodes.Status406NotAcceptable, "Index No Encontrado");
                                }
                            }
                            return StatusCode (StatusCodes.Status406NotAcceptable, "Materia No Encontrada");
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable, "No Existe Este Semestre");
                }

            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }
    }
}