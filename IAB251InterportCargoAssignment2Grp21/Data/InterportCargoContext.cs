using Microsoft.EntityFrameworkCore;
using IAB251InterportCargoAssignment2Grp21.Models;
using IAB251InterportCargoAssignment2Grp21.BusinessLogic.Entities;

namespace IAB251InterportCargoAssignment2Grp21.Data
{
    public class InterportCargoContext : DbContext
    {
        public InterportCargoContext(DbContextOptions<InterportCargoContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<LocalEmployeeCredential> LocalEmployeeCredentials { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationMessage> QuotationMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<LocalEmployeeCredential>().ToTable("Employee");
            modelBuilder.Entity<QuotationRequest>().ToTable("QuotationRequests");
            modelBuilder.Entity<Quotation>().ToTable("Quotations");
            modelBuilder.Entity<QuotationMessage>().ToTable("QuotationMessages");

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