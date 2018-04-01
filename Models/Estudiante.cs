using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrganizerU.Models {
    public class Estudiante {

        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }
        List<Semestre> Semestres { get; }
        public Estudiante (string id) {
            this.Id = id;
            Semestres = new List<Semestre> ();
        }
    }
}