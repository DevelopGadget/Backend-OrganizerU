using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
                    return BadRequest ("No Hay Documentos");
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
            return BadRequest ("No Hay Documentos");
        }

        [HttpGet ("Semestre/{Sem}")]
        public Task<IActionResult> GetSem (string UserId, int Sem) => GETSEM (UserId, Sem);
        private async Task<IActionResult> GETSEM (string UserId, int Sem) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Sem) {
                            return Ok (JsonConvert.SerializeObject (us.Materias));
                        }
                    }
                    return BadRequest ("No Existe Ese Semestre");
                }
            } catch (Exception e) {
                return BadRequest (e);
            }
            return BadRequest ("No Hay Documentos");
        }

        [HttpGet ("Id/{Id}")]
        public Task<IActionResult> Get (string UserId, string Id) => GET (UserId, Id);
        private async Task<IActionResult> GET (string UserId, string Id) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return BadRequest ("No Hay Documentos");
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
            return BadRequest ("No Hay Documentos");
        }

        [HttpPost ("{Semestre}")]
        public async Task<IActionResult> MateriasPost ([FromBody] Materia materia, string UserId, int Semestre) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest (ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            es.Semestres[Semestre - 1].Materias.Add (materia);
                            var h = await _estudiante.Update (UserId, es);
                            if (h.MatchedCount > 0) {
                                return Ok ("Creado");
                            } else {
                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                            }
                        }
                    }
                    return BadRequest ("No Existe Este Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpPost ("{Semestre}/{Id}")]
        public async Task<IActionResult> MateriasPut ([FromBody] Materia materia, string UserId, int Semestre, string Id) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest (ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            for (int i = 0; i < es.Semestres[Semestre - 1].Materias.Count; i++) {
                                if (es.Semestres[Semestre - 1].Materias[i].Id == Id) {
                                    es.Semestres[Semestre - 1].Materias[i] = materia;
                                    var h = await _estudiante.Update (UserId, es);
                                    if (h.MatchedCount > 0) {
                                        return Ok ("Modificado");
                                    } else {
                                        return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                                    }
                                }
                            }
                            return BadRequest ("Materia no encontrada");
                        }
                    }
                    return BadRequest ("No Existe Ese Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpDelete ("{Semestre}/{Id}")]
        public async Task<IActionResult> MateriasDelete (string UserId, int Semestre, string Id) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest (ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) {
                            var h = await _estudiante.Remove (Id);
                            if (h.DeletedCount > 0) {
                                return Ok ("Eliminado");
                            } else {
                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                            }
                        }
                    }
                    return BadRequest ("No Existe Ese Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }
    }
}