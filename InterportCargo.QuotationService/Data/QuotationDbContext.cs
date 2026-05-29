using InterportCargo.QuotationService.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.QuotationService.Data
{
    public class QuotationDbContext : DbContext
    {
        public QuotationDbContext(DbContextOptions<QuotationDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuotationRequest> QuotationRequests { get; set; }

        public DbSet<Quotation> Quotations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuotationRequest>().ToTable("QuotationRequests");
            modelBuilder.Entity<Quotation>().ToTable("Quotations");
        }
    }
}
