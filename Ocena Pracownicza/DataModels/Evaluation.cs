using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    public class Evaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationID { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string EvaluatorName { get; set; } //Nazwa ankiety do której będzie to wpisane (do sortowania u kierowników poprzez wybór oceny) ---automatic
        public DateTime Date { get; set; }
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }
        public string Question6 { get; set; }
    }
}
