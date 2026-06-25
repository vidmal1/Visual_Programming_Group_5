using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class ClassificationService
    {
        private readonly AppClassificationRepository _classificationRepository = new AppClassificationRepository();

        public async Task<List<AppClassificationRuleDto>> GetRulesAsync()
        {
            var rules = await _classificationRepository.GetAllAsync();

            return rules.Select(rule => new AppClassificationRuleDto
            {
                Id = rule.Id,
                ApplicationName = rule.ApplicationName,
                CategoryId = rule.CategoryId,
                CategoryName = rule.Category?.Name ?? "Neutral"
            }).ToList();
        }

        public async Task SaveRuleAsync(string applicationName, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentException("Application name is required.");
            }

            await _classificationRepository.AddOrUpdateAsync(applicationName, categoryId);
        }

        public async Task RemoveRuleAsync(int id)
        {
            await _classificationRepository.DeleteByIdAsync(id);
        }

        public async Task<int> GetCategoryIdForApplicationAsync(string applicationName)
        {
            int? categoryId = await _classificationRepository.GetCategoryIdForApplicationAsync(applicationName);

            return categoryId ?? 2; // Default = Neutral
        }
    }
}