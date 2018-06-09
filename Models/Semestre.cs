using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrganizerU.Models {
    public class Semestre : IValidatableObject {
        [Required]
        public List<Materia> Materias { get; set; }

        [Required]
        public int SemestreC { get; set; }

        [Required]
        public int Num_Cortes { get; set; }

        [Required]
        public int[] Porcentajes_Cortes { get; set; }
        public Semestre (int semestre, int num_Cortes, int[] Porcentajes_Cortes) {
            this.SemestreC = semestre;
            this.Num_Cortes = num_Cortes;
            this.Porcentajes_Cortes = Porcentajes_Cortes;
            this.Materias = new List<Materia> ();
        }
        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext) {
            var Err = new List<ValidationResult> ();
            int total = 0;
            if (SemestreC <= 0) {
                Err.Add (new ValidationResult ("El Semestre Tiene Que Ser Mayor A 0", new string[]{"1"}));
            }
            if (Num_Cortes <= 0) {
                Err.Add (new ValidationResult ("El Numero de cortes Tiene Que Ser Mayor A 0", new string[]{"2"}));
            }
            if (Porcentajes_Cortes.Length != Num_Cortes) {
                Err.Add (new ValidationResult ("El Tamaño Del Vector Tiene Que Ser Igual Al Numero De Cortes", new string[]{"3"}));
            }
            foreach(int i in Porcentajes_Cortes){
                total += i;
            }
            if(total != 100){
                Err.Add (new ValidationResult ("La Sumatoria Del Vector Tiene Que Ser Igual A 100", new string[]{"4"}));
            }
            return Err;
        }
    }
}