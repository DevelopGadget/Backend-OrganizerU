using System;
using System.ComponentModel.DataAnnotations;

namespace OrganizerU.Models {
    public class Dia {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }
        public Dia (string nombre, DateTime horaInicio, DateTime horaFin) {
            this.Nombre = nombre;
            this.HoraInicio = horaInicio;
            this.HoraFin = horaFin;
        }
    }
}