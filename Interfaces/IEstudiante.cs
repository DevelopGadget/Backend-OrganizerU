using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrganizerU.Models;

namespace OrganizerU.Interfaces
{

    public interface IEstudiante {
        Task<IEnumerable<Estudiante>> Get ();
        Task<Estudiante> Get (string _id);
        Task Add (Estudiante estudiante);
        Task<ReplaceOneResult> Update (string _id, Estudiante estudiante);
        Task<DeleteResult> Remove (string _id);
    }
}