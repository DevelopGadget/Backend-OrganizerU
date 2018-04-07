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
                if (await _estudiante.Get () == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
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

        [HttpGet ("{Semestre}")]
        public Task<IActionResult> GetSem (string UserId, int Semestre) => GETSEM (UserId, Semestre);
        private async Task<IActionResult> GETSEM (string UserId, int Semestre) {
            try {
                if (await _estudiante.Get () == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
                    foreach (Semestre us in es.Semestres) {
                        if (us.Semetre == Semestre) return BadRequest ("No Existe Ese Semestre");;
                    }
                     return Ok (JsonConvert.SerializeObject (es.Semestres[Semestre - 1].Materias));
                }
            } catch (Exception) {
                return BadRequest ("Hubo Un Error Vuelva Intentar");
            }
            return BadRequest ("No Hay Documentos");
        }

        [HttpGet ("{Id}")]
        public Task<IActionResult> Get (string UserId, string Id) => GET (UserId, Id);
        private async Task<IActionResult> GET (string UserId, string Id) {
            try {
                if (await _estudiante.Get () == null) {
                    return BadRequest ("No Hay Documentos");
                } else {
                    Estudiante es = await _estudiante.Get (UserId);
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
    }
}