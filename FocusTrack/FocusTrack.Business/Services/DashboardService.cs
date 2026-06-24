using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class DashboardService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();

        public async Task<DashboardSummaryDto> GetTodaySummaryAsync()
        {
            var todaySessions = await _sessionRepository.GetByDateAsync(DateTime.Today);

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

            return new DashboardSummaryDto
            {
                TotalSeconds = totalSeconds,
                TotalDurationText = FormatDuration(totalSeconds),
                SessionCount = todaySessions.Count,
                MostUsedApplication = mostUsedApp?.ApplicationName ?? "N/A",
                MostUsedDurationText = mostUsedApp?.DurationText ?? "0s",
                AppUsages = appUsages
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
