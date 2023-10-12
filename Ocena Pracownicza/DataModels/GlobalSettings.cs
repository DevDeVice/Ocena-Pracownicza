using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    public class GlobalSettings
    {
        [Key]
        public int Id { get; set; }
        public string CurrentEvaluationName { get; set; }
    }
}
