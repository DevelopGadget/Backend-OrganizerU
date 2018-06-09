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
    public class SemestreController : Controller {
        private readonly IEstudiante _estudiante;
        public SemestreController (IEstudiante _estudiante) => this._estudiante = _estudiante;
        [HttpGet]
        public Task<IActionResult> Get (string UserId) => GET (UserId);
        private async Task<IActionResult> GET (string UserId) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                } else {
                    if (es.Semestres == null) {
                        return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                    } else {
                        return Ok (JsonConvert.SerializeObject (es.Semestres));
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
        }

        [HttpGet ("{Semestre}")]
        public Task<IActionResult> Get (string UserId, int Semestre) => GET (UserId, Semestre);
        private async Task<IActionResult> GET (string UserId, int Semestre) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                } else {
                    Semestre sem = null;
                    foreach (Semestre ex in es.Semestres) {
                        if (ex.SemestreC == Semestre) sem = ex;
                    }
                    if (sem == null) {
                        return StatusCode (StatusCodes.Status406NotAcceptable,"No Hay Documentos");
                    } else {
                        return Ok (JsonConvert.SerializeObject (sem));
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Semestre semestre, string UserId) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    foreach (Semestre us in es.Semestres) {
                        if (us.SemestreC == semestre.SemestreC) return StatusCode (StatusCodes.Status406NotAcceptable,"Ya existe ese semestre");
                    }
                    es.Semestres.Add (semestre);
                    var h = await _estudiante.Update (UserId, es);
                    if (h.MatchedCount > 0) {
                        return Ok ("Creado");
                    } else {
                        return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                    }
                }
            } catch (Exception e) {
                return BadRequest (e);
            }
        }

        [HttpPut ("{Semestre}")]
        public async Task<IActionResult> Put (int Semestre, [FromBody] Semestre value, string UserId) {
            try {
                if (!ModelState.IsValid) {
                    return StatusCode (StatusCodes.Status406NotAcceptable,ModelState);
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                    for (int i = 0; i  < es.Semestres.Count; i++ )  {
                        if (es.Semestres[i].SemestreC == Semestre) {
                            foreach(Semestre us in es.Semestres){
                                if(us.SemestreC == value.SemestreC) return BadRequest ("Ya Existe Ese Semestre");
                            }
                            es.Semestres[i] = value;
                            var h = await _estudiante.Update (UserId, es);
                            if (h.MatchedCount > 0) {
                                return Ok ("Modificado");
                            } else {
                                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                            }
                        }
                    }
                    return StatusCode (StatusCodes.Status406NotAcceptable,"No Existe Ese Semestre");
                }
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
            }
        }

        [HttpDelete ("{Semestre}")]
        public async Task<IActionResult> Delete (int Semestre, string UserId) {
            try {
                Estudiante es = await _estudiante.Get (UserId);
                if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                foreach (Semestre us in es.Semestres) {
                    if (Semestre == us.SemestreC) {
                        es.Semestres.Remove (us);
                        var h = await _estudiante.Update (UserId, es);
                        if (h.MatchedCount > 0) {
                            return Ok ("Eliminado");
                        } else {
                            return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
                        }
                    }
                }
                return StatusCode (StatusCodes.Status406NotAcceptable,"Semestre no encontrado");
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva Intentar");
            }
        }
    }
}