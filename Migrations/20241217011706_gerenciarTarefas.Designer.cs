﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using gerenciadorTarefas.Data;

#nullable disable

namespace gerenciadorTarefas.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217011706_gerenciarTarefas")]
    partial class gerenciarTarefas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("gerenciadorTarefas.Models.Categoria", b =>
                {
                    b.Property<string>("CategoriaId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CategoriaId");

                    b.ToTable("Categorias");

                    b.HasData(
                        new
                        {
                            CategoriaId = "trabalho",
                            Nome = "Trabalho"
                        },
                        new
                        {
                            CategoriaId = "casa",
                            Nome = "Casa"
                        },
                        new
                        {
                            CategoriaId = "faculdade",
                            Nome = "Faculdade"
                        },
                        new
                        {
                            CategoriaId = "compras",
                            Nome = "Compras"
                        },
                        new
                        {
                            CategoriaId = "academia",
                            Nome = "Academia"
                        });
                });

            modelBuilder.Entity("gerenciadorTarefas.Models.Status", b =>
                {
                    b.Property<string>("StatusId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("StatusId");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            StatusId = "aberto",
                            Nome = "Aberto"
                        },
                        new
                        {
                            StatusId = "completo",
                            Nome = "Completo"
                        });
                });

            modelBuilder.Entity("gerenciadorTarefas.Models.Tarefa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoriaId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("DataDeVencimento")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StatusId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("StatusId");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("gerenciadorTarefas.Models.Tarefa", b =>
                {
                    b.HasOne("gerenciadorTarefas.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("gerenciadorTarefas.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}