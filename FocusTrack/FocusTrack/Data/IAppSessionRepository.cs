using FocusTrack.Models;

namespace FocusTrack.Data
{
    public interface IAppSessionRepository
    {
        Task<IReadOnlyList<DashboardSessionRecord>> GetSessionsAsync(DateTime from, DateTime to);

        Task<List<AppCategory>> GetAllCategoriesAsync();
        Task AddCategoryAsync(string categoryName);
        Task UpdateCategoryGoalAsync(int categoryId, int dailyGoalMinutes);
        Task<List<string>> GetIgnoreListAsync();
        Task AddToIgnoreListAsync(string appName);
        Task RemoveFromIgnoreListAsync(string appName);
        Task RemoveCategoryAsync(int categoryId);
        Task<List<AppClassification>> GetAppClassificationsAsync();
        Task SaveAppClassificationAsync(string appName, int categoryId);
    }
}
