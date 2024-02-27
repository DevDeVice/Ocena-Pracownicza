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
        public DbSet<EvaluationBiuro> EvaluationBiuro { get; set; }
        public DbSet<EvaluationProdukcja> EvaluationsProdukcja { get; set; }
        public DbSet<EvaluationName> EvaluationNames { get; set; }
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<EvaluationBiuroAnswer> EvaluationBiuroAnswers { get; set; }
        public DbSet<EvaluationProdukcjaAnswer> EvaluationProdukcjaAnswers { get; set; }
        public DbSet<Department> Department { get; set; }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer("Server=NAZWASERWERA;Database=ocena_pracownicza;User Id=sa;Password=;TrustServerCertificate=True;");//TODO ustawic nazwe przed startem :)
        }
    }
}
