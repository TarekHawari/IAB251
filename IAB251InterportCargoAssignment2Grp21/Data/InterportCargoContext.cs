using Microsoft.EntityFrameworkCore;
using IAB251InterportCargoAssignment2Grp21.Models;
// using FleetProLayered_Architecture.BusinessLogic.Entities;

namespace IAB251InterportCargoAssignment2Grp21.Data
{
    public class InterportCargoContext : DbContext
    {
        public InterportCargoContext(DbContextOptions <InterportCargoContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<LocalEmployeeCredential> LocalEmployeeCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<LocalEmployeeCredential>().ToTable("Employee");

            modelBuilder.Entity<LocalEmployeeCredential>().HasData(
              new LocalEmployeeCredential
              {
                  LocalEmployeeCredentialId = 1,
                  Email = "t.williams@company.com",
                  EmployeeKey = "QUOTE123"
              }
          );
        }


    }
}

