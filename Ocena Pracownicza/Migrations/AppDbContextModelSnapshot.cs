﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ocena_Pracownicza.DataModels;

#nullable disable

namespace Ocena_Pracownicza.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.EvaluationBiuro", b =>
                {
                    b.Property<int>("EvaluationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EvaluationAnswerID")
                        .HasColumnType("int");

                    b.Property<int>("EvaluatorNameID")
                        .HasColumnType("int");

                    b.Property<string>("Question1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question10")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question11")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question3")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question4")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question5")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question6")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question7")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question8")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question9")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EvaluationID");

                    b.ToTable("EvaluationBiuro");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.EvaluationBiuroAnswer", b =>
                {
                    b.Property<int>("EvaluationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Question1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question10")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question11")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question3")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question4")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question5")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question6")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question7")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question8")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question9")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EvaluationID");

                    b.ToTable("EvaluationBiuroAnswers");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.EvaluationName", b =>
                {
                    b.Property<int>("EvaluatorNameID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EvaluatorName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EvaluatorNameID");

                    b.ToTable("EvaluationNames");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.EvaluationProdukcja", b =>
                {
                    b.Property<int>("EvaluationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EvaluationAnswerID")
                        .HasColumnType("int");

                    b.Property<int>("EvaluatorNameID")
                        .HasColumnType("int");

                    b.Property<string>("Question1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question3")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question4")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question5")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EvaluationID");

                    b.ToTable("EvaluationsProdukcja");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.EvaluationProdukcjaAnswer", b =>
                {
                    b.Property<int>("EvaluationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Question1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question3")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Question4")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EvaluationID");

                    b.ToTable("EvaluationProdukcjaAnswers");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.GlobalSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CurrentEvaluationName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("GlobalSettings");
                });

            modelBuilder.Entity("Ocena_Pracownicza.DataModels.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
