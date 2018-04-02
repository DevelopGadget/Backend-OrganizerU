using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizerU.Models {
    public class Semestre {
        [Required]
        List<Materia> Materias { get; set; }
        [Required]
        private int Semetre { get; set; }
        [Required]
        private int Num_Cortes { get; set; }
        public Semestre (int semetre, int num_Cortes) {
            this.Semetre = semetre;
            this.Num_Cortes = num_Cortes;
            this.Materias = new List<Materia>();
        }
    }
}