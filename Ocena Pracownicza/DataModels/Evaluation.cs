using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    public class Evaluation
    {
        public int EvaluationID { get; set; } //Identyfikator ankiety ---automatic
        public int UserName { get; set; } //Imie nazwisko wypełniającego ---user
        public User User { get; set; } //Profil Kierownika - Do kogo przypisac ---user
        public string EvaluatorName { get; set; } //Nazwa ankiety do której będzie to wpisane (do sortowania u kierowników poprzez wybór oceny) ---automatic
        public DateTime Date { get; set; } //Data oceny ---automatic
        public string Question1 { get; set; } //pytanie ---automatic
        public string Question2 { get; set; } //pytanie ---automatic
        public string Question3 { get; set; } //pytanie ---automatic
        public string Question4 { get; set; } //pytanie ---automatic
        public string Question5 { get; set; } //pytanie ---automatic
        public string Question6 { get; set; } //pytanie ---automatic
    }
}
