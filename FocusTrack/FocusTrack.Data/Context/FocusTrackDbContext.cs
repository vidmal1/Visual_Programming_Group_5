using FocusTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Context
{
    public class FocusTrackDbContext : DbContext
    {
        private readonly string _dbPath;

        public FocusTrackDbContext()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string appFolder = Path.Combine(appDataPath, "FocusTrack");

            Directory.CreateDirectory(appFolder);

            _dbPath = Path.Combine(appFolder, "focustrack.db");
        }

        public DbSet<AppSession> AppSessions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DailyGoal> DailyGoals { get; set; }

        public DbSet<IgnoreListItem> IgnoreListItems { get; set; }

        public DbSet<AppClassificationRule> AppClassificationRules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Productive",
                    ColorHex = "#22C55E"
                },
                new Category
                {
                    Id = 2,
                    Name = "Neutral",
                    ColorHex = "#6B7280"
                },
                new Category
                {
                    Id = 3,
                    Name = "Distracting",
                    ColorHex = "#EF4444"
                }
            );

            modelBuilder.Entity<AppClassificationRule>()
    .HasIndex(rule => rule.ApplicationName)
    .IsUnique();
        }
    }
}