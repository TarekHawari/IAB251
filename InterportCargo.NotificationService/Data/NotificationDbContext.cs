using InterportCargo.NotificationService.Models;
using Microsoft.EntityFrameworkCore;

namespace InterportCargo.NotificationService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerNotification> CustomerNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerNotification>().ToTable("CustomerNotifications");
        }
    }
}
