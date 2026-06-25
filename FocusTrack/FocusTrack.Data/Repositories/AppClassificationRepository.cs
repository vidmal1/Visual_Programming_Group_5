using FocusTrack.Data.Context;
using FocusTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Repositories
{
    public class AppClassificationRepository
    {
        public async Task<List<AppClassificationRule>> GetAllAsync()
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.AppClassificationRules
                .Include(rule => rule.Category)
                .OrderBy(rule => rule.ApplicationName)
                .ToListAsync();
        }

        public async Task AddOrUpdateAsync(string applicationName, int categoryId)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            string normalizedName = applicationName.Trim();

            AppClassificationRule? existingRule = await context.AppClassificationRules
                .FirstOrDefaultAsync(rule => rule.ApplicationName.ToLower() == normalizedName.ToLower());

            if (existingRule == null)
            {
                AppClassificationRule rule = new AppClassificationRule
                {
                    ApplicationName = normalizedName,
                    CategoryId = categoryId
                };

                await context.AppClassificationRules.AddAsync(rule);
            }
            else
            {
                existingRule.CategoryId = categoryId;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            AppClassificationRule? rule = await context.AppClassificationRules.FindAsync(id);

            if (rule == null)
            {
                return;
            }

            context.AppClassificationRules.Remove(rule);
            await context.SaveChangesAsync();
        }

        public async Task<int?> GetCategoryIdForApplicationAsync(string applicationName)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            AppClassificationRule? rule = await context.AppClassificationRules
                .FirstOrDefaultAsync(rule => rule.ApplicationName.ToLower() == applicationName.ToLower());

            return rule?.CategoryId;
        }
    }
}