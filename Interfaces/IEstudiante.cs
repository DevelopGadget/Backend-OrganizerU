using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using OrganizerU.Models;

namespace OrganizerU.Interfaces {
    public interface IEstudiante {
        Task<IEnumerable<Estudiante>> Get ();
        Task<Estudiante> Get (string _id);
        Task Add (Estudiante estudiante);
        Task<ReplaceOneResult> Update (string _id, Estudiante estudiante);
        Task<DeleteResult> Remove (string _id);
        Task Archivo(string Filename, Stream stream, GridFSUploadOptions opt);
    }
}