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
    public class EstudianteController : Controller {
        private readonly IEstudiante _estudiante;

        public EstudianteController (IEstudiante _estudiante) => this._estudiante = _estudiante;

        [HttpGet]
        public Task<string> Get (string  UserId) => GET (UserId);

        private async Task<string> GET (string UserId) {
            if (await _estudiante.Get () == null) {
                return "No hay documentos";
            } else {
                return JsonConvert.SerializeObject (await _estudiante.Get (UserId));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put ([FromBody] Estudiante put, string UserId) {
            if (ModelState.IsValid) {
                put.Id = UserId;
                var h = await _estudiante.Update (UserId, put);
                if (h.MatchedCount > 0) {
                    return Ok ("Modificado");
                } else {
                    return BadRequest ("Hubo un error");
                }
            } else {
                return BadRequest (ModelState);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete (string UserId) {
            var e = await _estudiante.Remove (UserId);
            if (e.DeletedCount > 0) {
                return Ok ("Eliminado");
            } else {
                return BadRequest ("Hubo un error");
            }
        }
    }
}