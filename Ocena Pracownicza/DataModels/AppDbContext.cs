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
            optionsBuilder.UseMySql("server=localhost;database=ocena_pracownicza;user=root;password=",
                                     new MySqlServerVersion(new Version(10, 4, 28))); //new MySqlServerVersion(new Version(8, 2, 4)));
            //optionsBuilder.UseSqlServer("Server=192.168.1.11\\LANTEK;Database=ocena_pracownicza;User Id=sa;Password=;TrustServerCertificate=True;");//TODO ustawic nazwe przed startem :)
        }
    }
}
