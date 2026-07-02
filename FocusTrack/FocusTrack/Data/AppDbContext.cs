using Microsoft.EntityFrameworkCore;
using FocusTrack.Models;

namespace FocusTrack.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppSession> AppSessions { get; set; }
        public DbSet<AppCategory> AppCategories { get; set; }
        public DbSet<IgnoredApp> IgnoredApps { get; set; }
        public DbSet<AppClassification> AppClassifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=focustrack.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppCategory>()
                .HasIndex(category => category.Name)
                .IsUnique();

            modelBuilder.Entity<IgnoredApp>()
                .HasIndex(ignored => ignored.AppName)
                .IsUnique();

            modelBuilder.Entity<AppClassification>()
                .HasIndex(classification => classification.AppName)
                .IsUnique();

            modelBuilder.Entity<AppClassification>()
                .HasOne(classification => classification.Category)
                .WithMany()
                .HasForeignKey(classification => classification.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
