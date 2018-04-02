using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrganizerU.Models {
    public class Estudiante {

        [BsonId]
        [BsonRepresentation (BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        List<Semestre> Semestres { get; set; }
        public Estudiante (string id) {
            this.Id = id;
            Semestres = new List<Semestre> ();
        }
    }
}