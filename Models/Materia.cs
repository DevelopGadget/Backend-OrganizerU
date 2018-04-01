using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OrganizerU.Models {
  public class Materia {
    [Required]
    private DateTime HoraInicio { get; set; }

    [Required]
    private DateTime HoraFin { get; set; }

    [Required]
    private string[] Horario { get; set; }

    [Required]
    private string Nombre { get; set; }

    [Required]
    private string Profesor { get; set; }

    [Required]
    private string Salon { get; set; }

    [Required]
    private int Creditos { get; set; }

    [Required]
    private List<double> Cortes { get; set; }

    private List<Stream> Archivos { get; set; }
    public Materia (DateTime horaInicio, DateTime horaFin, string nombre, string profesor, string salon, int creditos) {
      this.HoraInicio = horaInicio;
      this.HoraFin = horaFin;
      this.Nombre = nombre;
      this.Profesor = profesor;
      this.Salon = salon;
      this.Creditos = creditos;
      this.Cortes = new List<double> ();
    }
  }
}