using System;
using System.ComponentModel.DataAnnotations;

namespace OrganizerU.Models {
    public class Dia {
        [Required]
        public string Nombre_Dia { get; set; }

        [Required]
        public DateTime HoraInicio { get; set; }

        [Required]
        public string Salon { get; set; }

        [Required]
        public DateTime HoraFin { get; set; }
        public Dia (string nombre, DateTime horaInicio, DateTime horaFin, string Salon) {
            this.Nombre_Dia = nombre;
            this.HoraInicio = horaInicio;
            this.HoraFin = horaFin;
            this.Salon  =  Salon;
        }
    }
}