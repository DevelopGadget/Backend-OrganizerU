using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Controllers {
    [Authorize]
    [Route ("[controller]/{UserId}")]
    public class MateriaController : Controller {
        private readonly IEstudiante _estudiante;
        public MateriaController (IEstudiante _estudiante) => this._estudiante = _estudiante;

        [HttpGet]
        public Task<IActionResult> Get (string UserId) => GET (UserId);
        private async Task<IActionResult> GET (string UserId) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                } else {
                    List<List<Materia>> Mat = new List<List<Materia>> ();
                    foreach (Semestre sem in es.Semestres) {
                        Mat.Add (sem.Materias);
                    }
                    return Ok (JsonConvert.SerializeObject (Mat));
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
        }

        [HttpGet ("Semestre/{Sem}")]
        public Task<IActionResult> GetSem (string UserId, int Sem) => GETSEM (UserId, Sem);
        private async Task<IActionResult> GETSEM (string UserId, int Sem) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                } else {
                    foreach (Semestre us in es.Semestres) {
                        if (us.SemestreC == Sem) {
                            return Ok (JsonConvert.SerializeObject (us.Materias));
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Existe Ese Semestre");
                }
            } catch (Exception e) {
                return BadRequest (e);
            }
        }

        [HttpGet ("Id/{Id}")]
        public Task<IActionResult> Get (string UserId, string Id) => GET (UserId, Id);
        private async Task<IActionResult> GET (string UserId, string Id) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                } else {
                    foreach (Semestre sem in es.Semestres) {
                        foreach (Materia Mat in sem.Materias) {
                            if (Mat.Id == Id) return Ok (JsonConvert.SerializeObject (Mat));
                        }
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
            return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
        }

        [HttpPost ("{Semestre}")]
        public async Task<IActionResult> MateriasPost ([FromBody] Materia materia, string UserId, int Semestre) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.SemestreC == Semestre) {
                            materia.Cortes_Notas = new List<double>[us.Num_Cortes];
                            for (int i = 0; i < us.Num_Cortes; i++) {
                                materia.Cortes_Notas[i] = new List<double> ();
                            }
                            for (int i = 0; i < materia.Horario.Length; i++) {
                                materia.Horario[i].HoraInicio = Convert.ToDateTime (materia.Horario[i].HoraInicio);
                                materia.Horario[i].HoraFin = Convert.ToDateTime (materia.Horario[i].HoraFin);
                            }
                            us.Materias.Add (materia);
                            var h = await _estudiante.Update (UserId, es);
                            if (h.MatchedCount > 0) {
                                return Ok ("Creado");
                            } else {
                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                            }
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Existe Este Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ah Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpPut ("{Semestre}/{Id}")]
        public async Task<IActionResult> MateriasPut ([FromBody] Materia materia, string UserId, int Semestre, string Id) {
            try {
                if (!ModelState.IsValid) {
                    return  StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.SemestreC == Semestre) {
                            for (int i = 0; i < us.Materias.Count; i++) {
                                if (us.Materias[i].Id == Id) {
                                    for (int j = 0; j < materia.Horario.Length; j++) {
                                        materia.Horario[j].HoraInicio = Convert.ToDateTime (materia.Horario[i].HoraInicio);
                                        materia.Horario[j].HoraFin = Convert.ToDateTime (materia.Horario[i].HoraFin);
                                    }
                                    materia.Id = us.Materias[i].Id;
                                    materia.Cortes_Notas = us.Materias[i].Cortes_Notas;
                                    materia.Archivos = us.Materias[i].Archivos;
                                    us.Materias[i] = materia;
                                    var h = await _estudiante.Update (UserId, es);
                                    if (h.MatchedCount > 0) {
                                        return Ok ("Modificado");
                                    } else {
                                        return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                    }
                                }
                            }
                            return StatusCode (StatusCodes.Status406NotAcceptable,"Materia no encontrada");
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Existe Ese Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
            }
        }

        [HttpDelete ("{Semestre}/{Id}")]
        public async Task<IActionResult> MateriasDelete (string UserId, int Semestre, string Id) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        foreach (Materia mat in us.Materias) {
                            if (us.SemestreC == Semestre && mat.Id == Id) {
                                us.Materias.Remove(mat);
                                var h = await _estudiante.Update(UserId, es);
                                if (h.MatchedCount > 0) {
                                    return Ok ("Eliminado");
                                } else {
                                    return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                }
                            }
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Existe Ese Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }
    }
}