using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizerU.Models {
  public class Materia {
    [Required]
    private DateTime HoraInicio { get; }

    [Required]
    private DateTime HoraFin { get; }

    [Required]
    private string[] Horario { get; }

    [Required]
    private string Nombre { get; }

    [Required]
    private string Profesor { get; }

    [Required]
    private string Salon { get; }

    [Required]
    private int Creditos { get; }

    [Required]
    private List<double> Cortes { get; }

    [Required]
    private List<double> Porcentajes { get; }
    
    public Materia (DateTime horaInicio, DateTime horaFin, string nombre, string profesor, string salon, int creditos, List<double> Cortes, List<double> Porcentajes) {
      this.HoraInicio = horaInicio;
      this.HoraFin = horaFin;
      this.Nombre = nombre;
      this.Profesor = profesor;
      this.Salon = salon;
      this.Creditos = creditos;
      this.Cortes = Cortes;
      this.Porcentajes = Porcentajes;
    }
  }
}