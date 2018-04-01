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
    }
}