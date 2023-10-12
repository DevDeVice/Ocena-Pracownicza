using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    [Keyless]
    public class GlobalSettings
    {
        public string CurrentEvaluationName { get; set; }
    }
}
