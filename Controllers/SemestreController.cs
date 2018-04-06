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
        [HttpGet ("{Semestre}")]
        public Task<IActionResult> Get (string UserId, int Semestre) => GET (UserId, Semestre);
        private async Task<IActionResult> GET (string UserId, int Semestre) {
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
            return BadRequest ("No Hay Documentos");
        }

    }
}