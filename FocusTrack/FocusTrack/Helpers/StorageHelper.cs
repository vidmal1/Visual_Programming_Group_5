using System;
using System.Collections.Generic;
using System.Linq;
using FocusTrack.Data;
using FocusTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Helpers
{
    public static class StorageHelper
    {
        public static void SaveSession(AppSession session)
        {
            if (session is null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            try
            {
                using var context = new AppDbContext();
                context.AppSessions.Add(session);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save the app session.", ex);
            }
        }

        public static List<AppSession> GetAllSessions()
        {
            try
            {
                using var context = new AppDbContext();
                return context.AppSessions
                    .Include(session => session.Category)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load app sessions.", ex);
            }
        }

        public static List<AppSession> GetTodaySessions()
        {
            try
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);

                using var context = new AppDbContext();
                return context.AppSessions
                    .Include(session => session.Category)
                    .Where(session => session.StartTime >= today && session.StartTime < tomorrow)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load today's sessions.", ex);
            }
        }

        public static List<AppCategory> GetAllCategories()
        {
            try
            {
                using var context = new AppDbContext();
                return context.AppCategories.OrderBy(category => category.Name).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load categories.", ex);
            }
        }

        public static AppCategory AddCategory(string categoryName, int dailyGoalMinutes)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category name is required.", nameof(categoryName));
            }

            using var context = new AppDbContext();
            var trimmedName = categoryName.Trim();

            var existingCategory = context.AppCategories
                .FirstOrDefault(category => category.Name.ToLower() == trimmedName.ToLower());

            if (existingCategory != null)
            {
                return existingCategory;
            }

            var categoryToAdd = new AppCategory
            {
                Name = trimmedName,
                DailyGoalMinutes = Math.Max(0, dailyGoalMinutes)
            };

            context.AppCategories.Add(categoryToAdd);
            context.SaveChanges();
            return categoryToAdd;
        }

        public static void UpdateCategoryGoal(int categoryId, int dailyGoalMinutes)
        {
            using var context = new AppDbContext();
            var category = context.AppCategories.Find(categoryId);
            if (category == null)
            {
                return;
            }

            category.DailyGoalMinutes = Math.Max(0, dailyGoalMinutes);
            context.SaveChanges();
        }

        public static void DeleteCategory(int categoryId)
        {
            using var context = new AppDbContext();
            var category = context.AppCategories.Find(categoryId);
            if (category == null)
            {
                return;
            }

            var hasSessions = context.AppSessions.Any(session => session.CategoryId == categoryId);
            if (hasSessions)
            {
                throw new InvalidOperationException("Cannot remove category with existing sessions.");
            }

            context.AppCategories.Remove(category);
            context.SaveChanges();
        }

        public static List<IgnoredApp> GetIgnoredApps()
        {
            using var context = new AppDbContext();
            return context.IgnoredApps.OrderBy(item => item.AppName).ToList();
        }

        public static void AddIgnoredApp(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            using var context = new AppDbContext();
            var normalizedName = appName.Trim();
            var exists = context.IgnoredApps.Any(item => item.AppName.ToLower() == normalizedName.ToLower());
            if (exists)
            {
                return;
            }

            context.IgnoredApps.Add(new IgnoredApp { AppName = normalizedName });
            context.SaveChanges();
        }

        public static void RemoveIgnoredApp(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            using var context = new AppDbContext();
            var ignored = context.IgnoredApps.FirstOrDefault(item => item.AppName.ToLower() == appName.Trim().ToLower());
            if (ignored == null)
            {
                return;
            }

            context.IgnoredApps.Remove(ignored);
            context.SaveChanges();
        }

        public static List<AppClassification> GetAppClassifications()
        {
            using var context = new AppDbContext();
            return context.AppClassifications
                .Include(classification => classification.Category)
                .OrderBy(classification => classification.AppName)
                .ToList();
        }

        public static void SaveAppClassification(string appName, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            using var context = new AppDbContext();
            var normalizedName = appName.Trim();
            var existing = context.AppClassifications
                .FirstOrDefault(classification => classification.AppName.ToLower() == normalizedName.ToLower());

            if (existing == null)
            {
                context.AppClassifications.Add(new AppClassification
                {
                    AppName = normalizedName,
                    CategoryId = categoryId
                });
            }
            else
            {
                existing.CategoryId = categoryId;
            }

            context.SaveChanges();
        }

        public static void RemoveAppClassification(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return;
            }

            using var context = new AppDbContext();
            var classification = context.AppClassifications
                .FirstOrDefault(item => item.AppName.ToLower() == appName.Trim().ToLower());

            if (classification == null)
            {
                return;
            }

            context.AppClassifications.Remove(classification);
            context.SaveChanges();
        }

        public static int ResolveCategoryId(string appName)
        {
            using var context = new AppDbContext();

            if (!string.IsNullOrWhiteSpace(appName))
            {
                var classification = context.AppClassifications
                    .FirstOrDefault(item => item.AppName.ToLower() == appName.Trim().ToLower());

                if (classification != null)
                {
                    return classification.CategoryId;
                }
            }

            var neutralCategory = context.AppCategories.FirstOrDefault(category => category.Name.ToLower() == "neutral");
            if (neutralCategory != null)
            {
                return neutralCategory.Id;
            }

            return AddCategory("Neutral", 0).Id;
        }

        public static bool IsIgnoredApp(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return false;
            }

            using var context = new AppDbContext();
            var normalizedName = appName.Trim().ToLower();
            return context.IgnoredApps.Any(item => item.AppName.ToLower() == normalizedName);
        }

        public static double GetTodayCategoryMinutes(int categoryId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            using var context = new AppDbContext();
            return context.AppSessions
                .Where(session => session.CategoryId == categoryId && session.StartTime >= today && session.StartTime < tomorrow)
                .AsEnumerable()
                .Sum(session => Math.Max(0, (session.EndTime - session.StartTime).TotalMinutes));
        }

        public static void EnsureDefaultCategories()
        {
            using var context = new AppDbContext();
            if (context.AppCategories.Any())
            {
                return;
            }

            context.AppCategories.AddRange(
                new AppCategory { Name = "Productive", DailyGoalMinutes = 480 },
                new AppCategory { Name = "Neutral", DailyGoalMinutes = 0 },
                new AppCategory { Name = "Distracting", DailyGoalMinutes = 60 },
                new AppCategory { Name = "Communication", DailyGoalMinutes = 60 },
                new AppCategory { Name = "Entertainment", DailyGoalMinutes = 0 }
            );
            context.SaveChanges();
        }

        public static void EnsureDefaultIgnoredApps()
        {
            using var context = new AppDbContext();
            if (context.IgnoredApps.Any())
            {
                return;
            }

            context.IgnoredApps.AddRange(
                new IgnoredApp { AppName = "explorer" },
                new IgnoredApp { AppName = "devenv" },
                new IgnoredApp { AppName = "taskmgr" }
            );
            context.SaveChanges();
        }
    }
}
