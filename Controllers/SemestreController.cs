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
    public class SemestreController : Controller {
        private readonly IEstudiante _estudiante;
        public SemestreController (IEstudiante _estudiante) => this._estudiante = _estudiante;
        [HttpGet]
        public Task<IActionResult> Get (string UserId) => GET (UserId);
        private async Task<IActionResult> GET (string UserId) {
            try {
                if (await _estudiante.Get () == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    if (es.Semestres == null) {
                        return BadRequest ("No Hay Documentos");
                    } else {
                        return Ok (JsonConvert.SerializeObject (es.Semestres));
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
            return BadRequest ("No Hay Documentos");
        }

        [HttpGet ("{Semestre}")]
        public Task<IActionResult> Get (string UserId, int Semestre) => GET (UserId, Semestre);
        private async Task<IActionResult> GET (string UserId, int Semestre) {
            try {
                if (await _estudiante.Get () == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    Semestre sem = null;
                    foreach (Semestre ex in es.Semestres) {
                        if (ex.Semetre == Semestre) sem = ex;
                    }
                    if (sem == null) {
                        return BadRequest ("No Hay Documentos");
                    } else {
                        return Ok (JsonConvert.SerializeObject (sem));
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
            return BadRequest ("No Hay Documentos");
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Semestre semestre, string UserId) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest (ModelState);
                } else {
                    if (await ExisteAsync (semestre: semestre, UserId: UserId)) {
                        Estudiante es = await _estudiante.Get (UserId);
                        es.Semestres.Add (semestre);
                        await _estudiante.Add (es);
                        return Ok ("Creado");
                    } else {
                        return BadRequest ("Ya existe ese semestre");
                    }
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
        }
        private async Task<bool> ExisteAsync (Semestre semestre, string UserId) {
            try {
                if (semestre == null) {
                    return false;
                }
                Estudiante es = await _estudiante.Get (UserId);
                foreach (Semestre us in es.Semestres) {
                    if (us.Semetre == semestre.Num_Cortes) return false;
                }
                return true;
            } catch (Exception) {
                return false;
            }

        }
    }
}