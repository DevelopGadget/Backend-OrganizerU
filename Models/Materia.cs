using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrganizerU.Models {
  public class Materia {
    [BsonId]
    [BsonRepresentation (BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public Dia[] Horario { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Profesor { get; set; }

    [Required]
    public string Salon { get; set; }

    [Required]
    public int Creditos { get; set; }

    [Required]
    public List<double>[] Cortes_Notas { get; set; }

    [Required] 
    public List<Stream> Archivos { get; set; }
    public Materia (string nombre, string profesor, string salon, int creditos, Dia[] Horario, int cortes) {
      this.Nombre = nombre;
      this.Profesor = profesor;
      this.Salon = salon;
      this.Creditos = creditos;
      this.Horario = Horario; 
      this.Cortes_Notas = new List<double>[cortes];
      this.Archivos = new List<Stream> ();
    }
  }
}