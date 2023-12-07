using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocena_Pracownicza.DataModels
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EvaluationBiuro> Evaluations { get; set; }
        public DbSet<EvaluationProdukcja> EvaluationsProdukcja { get; set; }
        public DbSet<EvaluationName> EvaluationNames { get; set; }
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<EvaluationBiuroAnswer> EvaluationBiuroAnswers { get; set; }
        public DbSet<EvaluationProdukcjaAnswer> EvaluationProdukcjaAnswers { get; set; }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=ocena_pracownicza;user=root;password=",
                                     new MySqlServerVersion(new Version(10, 4, 28))); //new MySqlServerVersion(new Version(8, 2, 4)));
        }
    }
}
