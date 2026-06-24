using FocusTrack.Business.DTOs;
using FocusTrack.Data.Repositories;

namespace FocusTrack.Business.Services
{
    public class HistoryService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();

        public async Task<List<SessionHistoryDto>> GetAllSessionsAsync()
        {
            var sessions = await _sessionRepository.GetAllAsync();

            return sessions.Select(session => new SessionHistoryDto
            {
                Id = session.Id,
                ApplicationName = session.ApplicationName,
                WindowTitle = session.WindowTitle,
                CategoryName = session.Category?.Name ?? "Neutral",
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                DurationSeconds = session.DurationSeconds,
                DurationText = FormatDuration(session.DurationSeconds)
            }).ToList();
        }

        public async Task<List<SessionHistoryDto>> GetFilteredSessionsAsync(
    DateTime fromDate,
    DateTime toDate,
    string? applicationName,
    int? categoryId)
        {
            var sessions = await _sessionRepository.GetFilteredAsync(
                fromDate,
                toDate,
                applicationName,
                categoryId
            );

            return sessions.Select(session => new SessionHistoryDto
            {
                Id = session.Id,
                ApplicationName = session.ApplicationName,
                WindowTitle = session.WindowTitle,
                CategoryName = session.Category?.Name ?? "Neutral",
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                DurationSeconds = session.DurationSeconds,
                DurationText = FormatDuration(session.DurationSeconds)
            }).ToList();
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