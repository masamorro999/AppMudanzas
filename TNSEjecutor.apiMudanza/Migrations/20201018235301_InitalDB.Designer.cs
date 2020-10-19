﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TNSEjecutor.apiMudanza.Data;

namespace TNSEjecutor.apiMudanza.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201018235301_InitalDB")]
    partial class InitalDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TNSEjecutor.Common.Entities.Ejecutor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Document")
                        .HasColumnType("int");

                    b.Property<string>("NWorkTrips")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TransacDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ejecutors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Document = 1020441,
                            NWorkTrips = "Case #1 = 2",
                            TransacDate = "10/18/2020 6:52:53 PM"
                        },
                        new
                        {
                            Id = 2,
                            Document = 43505,
                            NWorkTrips = "Case #1 = 22",
                            TransacDate = "10/18/2020 6:52:53 PM"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
