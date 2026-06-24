using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class DashboardService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();
        private readonly GoalRepository _goalRepository = new GoalRepository();

        public async Task<DashboardSummaryDto> GetTodaySummaryAsync()
        {
            var todaySessions = await _sessionRepository.GetByDateAsync(DateTime.Today);
            var dailyGoals = await _goalRepository.GetAllAsync();

            int totalSeconds = todaySessions.Sum(session => session.DurationSeconds);

            var appUsages = todaySessions
                .GroupBy(session => session.ApplicationName)
                .Select(group => new AppUsageDto
                {
                    ApplicationName = group.Key,
                    TotalSeconds = group.Sum(session => session.DurationSeconds),
                    DurationText = FormatDuration(group.Sum(session => session.DurationSeconds))
                })
                .OrderByDescending(app => app.TotalSeconds)
                .ToList();

            var mostUsedApp = appUsages.FirstOrDefault();

            var goalProgresses = dailyGoals
                .Select(goal =>
                {
                    int usedSeconds = todaySessions
                        .Where(session => session.CategoryId == goal.CategoryId)
                        .Sum(session => session.DurationSeconds);

                    int goalSeconds = goal.GoalMinutes * 60;
                    int remainingSeconds = Math.Max(0, goalSeconds - usedSeconds);

                    int progressPercentage = goalSeconds == 0
                        ? 0
                        : Math.Min(100, (int)((usedSeconds / (double)goalSeconds) * 100));

                    return new DashboardGoalProgressDto
                    {
                        CategoryName = goal.Category?.Name ?? "Unknown",
                        GoalMinutes = goal.GoalMinutes,
                        UsedSeconds = usedSeconds,
                        UsedText = FormatDuration(usedSeconds),
                        RemainingText = FormatDuration(remainingSeconds),
                        ProgressPercentage = progressPercentage
                    };
                })
                .ToList();

            return new DashboardSummaryDto
            {
                TotalSeconds = totalSeconds,
                TotalDurationText = FormatDuration(totalSeconds),
                SessionCount = todaySessions.Count,
                MostUsedApplication = mostUsedApp?.ApplicationName ?? "N/A",
                MostUsedDurationText = mostUsedApp?.DurationText ?? "0s",
                AppUsages = appUsages,
                GoalProgresses = goalProgresses
            };
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