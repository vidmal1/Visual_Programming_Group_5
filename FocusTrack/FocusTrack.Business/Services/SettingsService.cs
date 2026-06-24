using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class SettingsService
    {
        private readonly CategoryRepository _categoryRepository = new CategoryRepository();
        private readonly GoalRepository _goalRepository = new GoalRepository();
        private readonly IgnoreListRepository _ignoreListRepository = new IgnoreListRepository();

        public async Task<List<CategorySettingDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(category => new CategorySettingDto
            {
                Id = category.Id,
                Name = category.Name,
                ColorHex = category.ColorHex
            }).ToList();
        }

        public async Task<List<DailyGoalDto>> GetDailyGoalsAsync()
        {
            var goals = await _goalRepository.GetAllAsync();

            return goals.Select(goal => new DailyGoalDto
            {
                Id = goal.Id,
                CategoryId = goal.CategoryId,
                CategoryName = goal.Category?.Name ?? "Unknown",
                GoalMinutes = goal.GoalMinutes
            }).ToList();
        }

        public async Task SaveDailyGoalAsync(int categoryId, int goalMinutes)
        {
            if (goalMinutes < 0)
            {
                throw new ArgumentException("Goal minutes cannot be negative.");
            }

            await _goalRepository.SaveGoalAsync(categoryId, goalMinutes);
        }

        public async Task<List<IgnoreListItemDto>> GetIgnoredAppsAsync()
        {
            var ignoredApps = await _ignoreListRepository.GetAllAsync();

            return ignoredApps.Select(item => new IgnoreListItemDto
            {
                Id = item.Id,
                ApplicationName = item.ApplicationName
            }).ToList();
        }

        public async Task AddIgnoredAppAsync(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentException("Application name is required.");
            }

            await _ignoreListRepository.AddAsync(applicationName.Trim());
        }

        public async Task RemoveIgnoredAppAsync(int id)
        {
            await _ignoreListRepository.DeleteByIdAsync(id);
        }
    }
}