using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    public class Evaluation
    {
        public int EvaluationID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public string EvaluatorName { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Comments { get; set; }
    }
}
