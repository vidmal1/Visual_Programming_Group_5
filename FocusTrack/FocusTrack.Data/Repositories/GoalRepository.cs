using FocusTrack.Data.Context;
using FocusTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusTrack.Data.Repositories
{
    public class GoalRepository
    {
        public async Task<List<DailyGoal>> GetAllAsync()
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            return await context.DailyGoals
                .Include(goal => goal.Category)
                .OrderBy(goal => goal.CategoryId)
                .ToListAsync();
        }

        public async Task SaveGoalAsync(int categoryId, int goalMinutes)
        {
            using FocusTrackDbContext context = new FocusTrackDbContext();

            DailyGoal? existingGoal = await context.DailyGoals
                .FirstOrDefaultAsync(goal => goal.CategoryId == categoryId);

            if (existingGoal == null)
            {
                DailyGoal newGoal = new DailyGoal
                {
                    CategoryId = categoryId,
                    GoalMinutes = goalMinutes
                };

                await context.DailyGoals.AddAsync(newGoal);
            }
            else
            {
                existingGoal.GoalMinutes = goalMinutes;
            }

            await context.SaveChangesAsync();
        }
    }
}