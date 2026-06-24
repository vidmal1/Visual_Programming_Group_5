using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class ReportService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();

        public async Task<List<ReportRowDto>> GetReportAsync(
            DateTime fromDate,
            DateTime toDate,
            string groupBy)
        {
            var sessions = await _sessionRepository.GetFilteredAsync(
                fromDate,
                toDate,
                null,
                null
            );

            if (groupBy == "Category")
            {
                return sessions
                    .GroupBy(session => session.Category?.Name ?? "Neutral")
                    .Select(group => new ReportRowDto
                    {
                        GroupName = group.Key,
                        SessionCount = group.Count(),
                        TotalSeconds = group.Sum(session => session.DurationSeconds),
                        TotalDurationText = FormatDuration(group.Sum(session => session.DurationSeconds))
                    })
                    .OrderByDescending(row => row.TotalSeconds)
                    .ToList();
            }

            return sessions
                .GroupBy(session => session.ApplicationName)
                .Select(group => new ReportRowDto
                {
                    GroupName = group.Key,
                    SessionCount = group.Count(),
                    TotalSeconds = group.Sum(session => session.DurationSeconds),
                    TotalDurationText = FormatDuration(group.Sum(session => session.DurationSeconds))
                })
                .OrderByDescending(row => row.TotalSeconds)
                .ToList();
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