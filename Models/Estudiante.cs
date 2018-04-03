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
        public Estudiante (string id, List<Semestre> Semestres) {
            this.Id = id;
            this.Semestres = new List<Semestre> (Semestres);
            this.Semestres.Add(new Semestre(1,3));
        }
    }
}