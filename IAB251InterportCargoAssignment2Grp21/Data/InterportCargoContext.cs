using Microsoft.EntityFrameworkCore;
using IAB251InterportCargoAssignment2Grp21.Models;
// using FleetProLayered_Architecture.BusinessLogic.Entities;

namespace IAB251InterportCargoAssignment2Grp21.Data
{
    public class InterportCargoContext :DbContext
    {
        public InterportCargoContext(DbContextOptions <InterportCargoContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        // public DbSet<Employee> Employee { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Customer>().ToTable("Customer");
        //     modelBuilder.Entity<Employee>().ToTable("Employee");
        // }


    }
}

