using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;
using OrganizerU.Interfaces;
using OrganizerU.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace OrganizerU.Controllers
{
    [Authorize]
    [Route("[controller]/{UserId}")]
    public class ArchivosController : Controller
    {

        private readonly IEstudiante _estudiante;
        private readonly IArchivo _archivo;

        public ArchivosController(IEstudiante _estudiante, IArchivo _archivo)
        {
            this._estudiante = _estudiante;
            this._archivo = _archivo;
        }

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
                            List<GridFSFileInfo> Ar = new List<GridFSFileInfo>();
                            foreach(ObjectId Id in mat.Archivos)
                            {
                                Ar.Add(await _archivo.Get(Id));
                            }
                            return Ok(JsonConvert.SerializeObject(Ar));
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
                                List<GridFSFileInfo> Ar = new List<GridFSFileInfo>();
                                foreach (ObjectId Idm in mat.Archivos)
                                {
                                    Ar.Add(await _archivo.Get(Idm));
                                }
                                return Ok(JsonConvert.SerializeObject(Ar));
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
                if (!ModelState.IsValid) return StatusCode(StatusCodes.Status406NotAcceptable, ModelState);
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
                                if (FilePost.Length > 0 && FilePost.Length < (2000 * 1024))
                                {
                                    var options = new GridFSUploadOptions
                                    {
                                        Metadata = new BsonDocument
                                        {
                                            {"Id Materia",  Id}
                                        }
                                    };
                                    var Idm  = await _archivo.Add(FilePost.FileName, FilePost.OpenReadStream(), options);
                                    if (Idm == null)
                                    {
                                        return BadRequest("Ha Ocurrido Un Error Vuelva Intentar");
                                    }
                                    mat.Archivos.Add(Idm);
                                    var h = await _estudiante.Update(UserId, es);
                                    if (h.MatchedCount > 0)
                                    {
                                        return Ok("Creado");
                                    }
                                    else
                                    {
                                        return BadRequest("Ha Ocurrido Un Error Vuelva Intentar");
                                    }
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
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id, string UserId)
        {
            try
            {
                if (Id.Length < 24)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                }
                Estudiante es = await _estudiante.Get(UserId);
                if (es == null) return StatusCode(StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                foreach (Semestre us in es.Semestres)
                {
                    foreach(Materia mat in us.Materias)
                    {
                        foreach (ObjectId Ids in mat.Archivos)
                        {
                            if(new ObjectId(Id) == Ids)
                            {
                                await _archivo.Delete(Ids);
                                mat.Archivos.Remove(Ids);
                                var h = await _estudiante.Update(UserId, es);
                                if (h.MatchedCount > 0)
                                {
                                    return Ok("Eliminado");
                                }
                                else
                                {
                                    return BadRequest("Ha Ocurrido Un Error Vuelva Intentar");
                                }
                            }
                        }
                    }
                }
                return StatusCode(StatusCodes.Status406NotAcceptable, "Id no encontrado");
            }
            catch (Exception)
            {
                return BadRequest("Ha Ocurrido Un Error Vuelva Intentar");
            }
        }
    }
}