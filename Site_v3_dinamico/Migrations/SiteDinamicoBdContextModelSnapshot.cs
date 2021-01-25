﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Site_v3_dinamico.Data;

namespace Site_v3_dinamico.Migrations
{
    [DbContext(typeof(SiteDinamicoBdContext))]
    partial class SiteDinamicoBdContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Site_v3_dinamico.Models.Competencias", b =>
                {
                    b.Property<int>("CompetenciasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("descricaoComp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("nivelComp")
                        .HasColumnType("int");

                    b.Property<string>("nomeComp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nomeLinguagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompetenciasId");

                    b.ToTable("Competencias");
                });

            modelBuilder.Entity("Site_v3_dinamico.Models.Exp_Profissional", b =>
                {
                    b.Property<int>("Exp_ProfissionalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("dataFim")
                        .HasColumnType("int");

                    b.Property<int>("dataInicio")
                        .HasColumnType("int");

                    b.Property<int>("descricaoFuncao")
                        .HasColumnType("int");

                    b.Property<int>("funcao")
                        .HasColumnType("int");

                    b.Property<string>("nomeEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Exp_ProfissionalId");

                    b.ToTable("Exp_Profissional");
                });

            modelBuilder.Entity("Site_v3_dinamico.Models.Formacao", b =>
                {
                    b.Property<int>("FormacaoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("conteudos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dataIniciodataFim")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nomeCurso")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nomeInstituicao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FormacaoId");

                    b.ToTable("Formacao");
                });

            modelBuilder.Entity("Site_v3_dinamico.Models.FormacaoComp", b =>
                {
                    b.Property<int>("FormacaoCompId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("dataIniciodataFimComp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nomeCursoComp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nomeInstituicaoComp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FormacaoCompId");

                    b.ToTable("FormacaoComp");
                });
#pragma warning restore 612, 618
        }
    }
}
