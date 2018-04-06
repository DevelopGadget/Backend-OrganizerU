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
    public DateTime HoraInicio { get; set; }

    [Required]
    public DateTime HoraFin { get; set; }

    [Required]
    public string[] Horario { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Profesor { get; set; }

    [Required]
    public string Salon { get; set; }

    [Required]
    public int Creditos { get; set; }

    [Required]
    public List<List<double>> Cortes_Notas { get; set; }

    public List<Stream> Archivos { get; set; }
    public Materia (DateTime horaInicio, DateTime horaFin, string nombre, string profesor, string salon, int creditos) {
      this.HoraInicio = horaInicio;
      this.HoraFin = horaFin;
      this.Nombre = nombre;
      this.Profesor = profesor;
      this.Salon = salon;
      this.Creditos = creditos;
      this.Cortes_Notas = new List<List<double>>();
      this.Archivos = new List<Stream> ();
    }
  }
}