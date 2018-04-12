using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using OrganizerU.Interfaces;
using OrganizerU.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrganizerU.Controllers
{
    [Authorize]
    [Route("[controller]/{UserId}")]
    public class ArchivosController : Controller
    {

        private readonly IEstudiante _estudiante;
        public ArchivosController(IEstudiante _estudiante) => this._estudiante = _estudiante;

        [HttpGet]
        public Task<IActionResult> Get(string UserId) => GET(UserId);
        private async Task<IActionResult> GET(string UserId)
        {
            try
            {
                Estudiante es = await _estudiante.Get(UserId);
                if (es == null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                }
                else
                {
                    foreach (Semestre us in es.Semestres)
                    {
                        foreach (Materia mat in us.Materias)
                        {
                            return Ok(JsonConvert.SerializeObject(mat.Archivos));
                        }
                    }
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Existe Esa Materia");
                }
            }
            catch (Exception)
            {
                return BadRequest("Hubo Un Error Vuelva Intentar");
            }
        }
        [HttpGet("{Id}")]
        public Task<IActionResult> Get(string UserId, string Id) => GetMat(UserId, Id);
        private async Task<IActionResult> GetMat(string UserId, string Id)
        {
            try
            {
                Estudiante es = await _estudiante.Get(UserId);
                if (es == null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                }
                else
                {
                    foreach (Semestre us in es.Semestres)
                    {
                        foreach (Materia mat in us.Materias)
                        {
                            if (mat.Id.Equals(Id))
                            {
                                return Ok(JsonConvert.SerializeObject(mat.Archivos));
                            }
                        }
                    }
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Existe Esa Materia");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpPost("{Id}")]
        public async Task<IActionResult> Post(IFormFile FilePost, string Id, string UserId)
        {
            try
            {
                Estudiante es = await _estudiante.Get(UserId);
                if (es == null || FilePost == null)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                }
                else
                {
                    foreach (Semestre us in es.Semestres)
                    {
                        foreach (Materia mat in us.Materias)
                        {
                            if (mat.Id.Equals(Id))
                            {
                                if (FilePost.Length > 0 && FilePost.Length < 2097152)
                                {
                                    var options = new GridFSUploadOptions
                                    {
                                        Metadata = new BsonDocument
                                        {
                                            {"Id Materia",  Id}
                                        }
                                    };
                                    await _estudiante.Archivo(FilePost.FileName, FilePost.OpenReadStream(), options);
                                    return Ok("Creado");
                                }
                                else
                                {
                                    return StatusCode(StatusCodes.Status406NotAcceptable, "Archivos menores a 2 mb");
                                }
                            }
                        }
                    }
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Existe Esa Materia");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}