using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    public class EvaluationBiuro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationID { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public int EvaluatorNameID { get; set; } 
        public DateTime Date { get; set; }
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }
        public string Question6 { get; set; }
        public string Question7 { get; set; }
        public string Question8 { get; set; }
        public string Question9 { get; set; }
        public string Question10 { get; set; }
        public string Question11 { get; set; }
        public int EvaluationAnswerID { get; set; }
        public int DepartmentID { get; set; }
    }
}
