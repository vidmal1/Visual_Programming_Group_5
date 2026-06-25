using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class NotificationService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();
        private readonly GoalRepository _goalRepository = new GoalRepository();

        private readonly HashSet<int> _notifiedCategoryIds = new HashSet<int>();
        private readonly Dictionary<int, int> _lastUsedSecondsByCategory = new Dictionary<int, int>();

        private DateTime _notificationDate = DateTime.Today;
        private bool _isInitialized = false;

        public async Task<List<GoalAlertDto>> CheckGoalAlertsAsync()
        {
            ResetNotificationsIfNewDay();

            var todaySessions = await _sessionRepository.GetByDateAsync(DateTime.Today);
            var dailyGoals = await _goalRepository.GetAllAsync();

            List<GoalAlertDto> alerts = new List<GoalAlertDto>();

            foreach (var goal in dailyGoals)
            {
                if (goal.GoalMinutes <= 0)
                {
                    continue;
                }

                int usedSeconds = todaySessions
                    .Where(session => session.CategoryId == goal.CategoryId)
                    .Sum(session => session.DurationSeconds);

                int goalSeconds = goal.GoalMinutes * 60;

                if (!_lastUsedSecondsByCategory.ContainsKey(goal.CategoryId))
                {
                    _lastUsedSecondsByCategory[goal.CategoryId] = usedSeconds;
                }

                int previousUsedSeconds = _lastUsedSecondsByCategory[goal.CategoryId];

                if (!_isInitialized)
                {
                    _lastUsedSecondsByCategory[goal.CategoryId] = usedSeconds;
                    continue;
                }

                bool crossedGoalNow =
                    previousUsedSeconds < goalSeconds &&
                    usedSeconds >= goalSeconds;

                if (crossedGoalNow && !_notifiedCategoryIds.Contains(goal.CategoryId))
                {
                    string categoryName = goal.Category?.Name ?? "Unknown";
                    string usedText = FormatDuration(usedSeconds);

                    GoalAlertDto alert = new GoalAlertDto
                    {
                        CategoryId = goal.CategoryId,
                        CategoryName = categoryName,
                        GoalMinutes = goal.GoalMinutes,
                        UsedText = usedText,
                        Message = $"{categoryName} daily goal reached! Used: {usedText}"
                    };

                    alerts.Add(alert);
                    _notifiedCategoryIds.Add(goal.CategoryId);
                }

                _lastUsedSecondsByCategory[goal.CategoryId] = usedSeconds;
            }

            _isInitialized = true;

            return alerts;
        }

        private void ResetNotificationsIfNewDay()
        {
            if (_notificationDate.Date != DateTime.Today)
            {
                _notifiedCategoryIds.Clear();
                _lastUsedSecondsByCategory.Clear();

                _notificationDate = DateTime.Today;
                _isInitialized = false;
            }
        }

        private string FormatDuration(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

            if (time.TotalHours >= 1)
            {
                return $"{(int)time.TotalHours}h {time.Minutes}m";
            }

            if (time.TotalMinutes >= 1)
            {
                return $"{time.Minutes}m {time.Seconds}s";
            }

            return $"{time.Seconds}s";
        }
    }
}