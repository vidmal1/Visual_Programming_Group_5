using FocusTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data
{
    public sealed class AppSessionRepository : IAppSessionRepository
    {
        public async Task<IReadOnlyList<DashboardSessionRecord>> GetSessionsAsync(DateTime from, DateTime to)
        {
            await using var db = new AppDbContext();

            return await db.AppSessions
                .AsNoTracking()
                .Include(session => session.Category)
                .Where(session => session.StartTime < to && session.EndTime >= from)
                .OrderByDescending(session => session.StartTime)
                .Select(session => new DashboardSessionRecord
                {
                    Id = session.Id,
                    AppName = session.AppName,
                    WindowTitle = session.WindowTitle,
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    CategoryName = session.Category != null ? session.Category.Name : "Neutral",
                    DailyGoalMinutes = session.Category != null ? session.Category.DailyGoalMinutes : 0
                })
                .ToListAsync();
        }

        public async Task<List<AppCategory>> GetAllCategoriesAsync()
        {
            using var db = new AppDbContext();
            return await db.AppCategories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task AddCategoryAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return;
            using var db = new AppDbContext();
            var exists = await db.AppCategories.AnyAsync(c => c.Name.ToLower() == categoryName.Trim().ToLower());
            if (exists)
            {
                throw new InvalidOperationException($"A category named '{categoryName}' already exists.");
            }
            db.AppCategories.Add(new AppCategory { Name = categoryName.Trim(), DailyGoalMinutes = 0 });
            await db.SaveChangesAsync();
        }

        public async Task UpdateCategoryGoalAsync(int categoryId, int dailyGoalMinutes)
        {
            using var db = new AppDbContext();
            var category = await db.AppCategories.FindAsync(categoryId);
            if (category != null)
            {
                category.DailyGoalMinutes = dailyGoalMinutes;
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetIgnoreListAsync()
        {
            await using var db = new AppDbContext();
            return await db.IgnoredApps
                .OrderBy(a => a.AppName)
                .Select(a => a.AppName)
                .ToListAsync();
        }

        public async Task AddToIgnoreListAsync(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName)) return;
            await using var db = new AppDbContext();
            var normalizedName = appName.Trim();
            var exists = await db.IgnoredApps.AnyAsync(item => item.AppName.ToLower() == normalizedName.ToLower());
            if (!exists)
            {
                db.IgnoredApps.Add(new IgnoredApp { AppName = normalizedName });
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveFromIgnoreListAsync(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName)) return;
            await using var db = new AppDbContext();
            var item = await db.IgnoredApps.FirstOrDefaultAsync(x => x.AppName.ToLower() == appName.Trim().ToLower());
            if (item != null)
            {
                db.IgnoredApps.Remove(item);
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await using var db = new AppDbContext();
            var category = await db.AppCategories.FindAsync(categoryId);
            if (category != null)
            {
                var neutralCategory = await db.AppCategories.FirstOrDefaultAsync(c => c.Name.ToLower() == "neutral");
                if (neutralCategory != null && neutralCategory.Id != categoryId)
                {
                    var sessions = await db.AppSessions.Where(s => s.CategoryId == categoryId).ToListAsync();
                    foreach (var session in sessions)
                    {
                        session.CategoryId = neutralCategory.Id;
                    }

                    var classifications = await db.AppClassifications.Where(c => c.CategoryId == categoryId).ToListAsync();
                    foreach (var classification in classifications)
                    {
                        classification.CategoryId = neutralCategory.Id;
                    }
                }

                db.AppCategories.Remove(category);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<AppClassification>> GetAppClassificationsAsync()
        {
            await using var db = new AppDbContext();
            
            var ignoredApps = new HashSet<string>(
                await db.IgnoredApps.Select(i => i.AppName.ToLower()).ToListAsync(),
                StringComparer.OrdinalIgnoreCase
            );

            var classifications = await db.AppClassifications
                .Include(c => c.Category)
                .Where(c => !ignoredApps.Contains(c.AppName))
                .ToListAsync();

            var classifiedAppNames = new HashSet<string>(
                classifications.Select(c => c.AppName.ToLowerInvariant())
            );

            var sessionAppNames = await db.AppSessions
                .Select(s => s.AppName)
                .Distinct()
                .ToListAsync();

            var neutralCategory = await db.AppCategories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == "neutral")
                ?? await db.AppCategories.FirstOrDefaultAsync();

            foreach (var appName in sessionAppNames)
            {
                var lowerAppName = appName.ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(appName) && 
                    !classifiedAppNames.Contains(lowerAppName) && 
                    !ignoredApps.Contains(lowerAppName))
                {
                    classifications.Add(new AppClassification
                    {
                        AppName = appName,
                        CategoryId = neutralCategory?.Id ?? 0,
                        Category = neutralCategory!
                    });
                }
            }

            return classifications.OrderBy(c => c.AppName).ToList();
        }

        public async Task SaveAppClassificationAsync(string appName, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(appName)) return;
            await using var db = new AppDbContext();
            var normalizedName = appName.Trim();
            var existing = await db.AppClassifications
                .FirstOrDefaultAsync(c => c.AppName.ToLower() == normalizedName.ToLower());

            if (existing == null)
            {
                db.AppClassifications.Add(new AppClassification
                {
                    AppName = normalizedName,
                    CategoryId = categoryId
                });
            }
            else
            {
                existing.CategoryId = categoryId;
            }

            await db.SaveChangesAsync();
        }
    }
}
