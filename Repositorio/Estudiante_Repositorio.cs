using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrganizerU.Interfaces;
using OrganizerU.Models;

namespace OrganizerU.Repositorio
{
    public class Estudiante_Repositorio : IEstudiante {
        private readonly ObjectContext context = null;
        public Estudiante_Repositorio (IOptions<Settings> settings) => context = new ObjectContext (settings);
        public async Task Add (Estudiante estudiante) => await context.Estudiante.InsertOneAsync (estudiante); 
        public async Task<IEnumerable<Estudiante>> Get () => await context.Estudiante.Find (x => true).ToListAsync ();
        public async Task<Estudiante> Get (string _id) => await context.Estudiante.Find (Builders<Estudiante>.Filter.Eq ("Id", _id)).FirstOrDefaultAsync ();
        public async Task<DeleteResult> Remove (string _id) => await context.Estudiante.DeleteOneAsync (Builders<Estudiante>.Filter.Eq ("Id", _id));
        public async Task<ReplaceOneResult> Update (string _id, Estudiante estudiante) => await context.Estudiante.ReplaceOneAsync (o => o.Id.Equals (_id), estudiante);
    }
}