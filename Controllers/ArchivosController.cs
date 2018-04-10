using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerU.Interfaces;

namespace OrganizerU.Controllers {
    [Authorize]
    [Route ("[controller]/{UserId}")]
    public class ArchivosController : Controller {
        private readonly IEstudiante _estudiante;

        public ArchivosController (IEstudiante _estudiante) => this._estudiante = _estudiante;
    }
}