using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNSEjecutor.Common.Entities;

namespace TNSEjecutor.apiMudanza.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Ejecutor> Ejecutors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ejecutor>()
                .HasData(
                new Ejecutor 
                { 
                    Id = 1, Document = 1020441, 
                    TransacDate = DateTime.Now.ToString(), 
                    NWorkTrips = "Case #1 = 2" 
                },
                new Ejecutor
                {
                    Id = 2,
                    Document = 43505,
                    TransacDate = DateTime.Now.ToString(),
                    NWorkTrips = "Case #1 = 22"
                });

            //modelBuilder.Entity<Ejecutor>()
            //    .HasIndex(t => t.Document)
            //    .IsUnique();
        }

    }
}
