using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OrganizerU.Models {
  public class Materia {
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
    public List<double> Cortes { get; set; }

    public List<Stream> Archivos { get; set; }
    public Materia (DateTime horaInicio, DateTime horaFin, string nombre, string profesor, string salon, int creditos) {
      this.HoraInicio = horaInicio;
      this.HoraFin = horaFin;
      this.Nombre = nombre;
      this.Profesor = profesor;
      this.Salon = salon;
      this.Creditos = creditos;
      this.Cortes = new List<double> ();
      this.Archivos= new List<Stream>();
    }
  }
}