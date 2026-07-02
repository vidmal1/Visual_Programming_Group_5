using FocusTrack.Data;

namespace FocusTrack.Services
{
    public sealed class DashboardService : IDashboardService
    {
        private static readonly string[] DefaultCategories = { "Productive", "Neutral", "Distracting", "Communication", "Entertainment" };
        private readonly IAppSessionRepository sessionRepository;

        public DashboardService()
            : this(new AppSessionRepository())
        {
        }

        public DashboardService(IAppSessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository;
        }

        public async Task<DashboardReport> GetDailyReportAsync(DateTime date)
        {
            var from = date.Date;
            var to = from.AddDays(1);
            var sessions = await sessionRepository.GetSessionsAsync(from, to);

            var recentSessions = sessions
                .Select(session => new RecentSessionSummary
                {
                    AppName = session.AppName,
                    WindowTitle = session.WindowTitle,
                    CategoryName = NormalizeCategory(session.CategoryName),
                    StartTime = session.StartTime,
                    EndTime = session.EndTime,
                    DurationMinutes = CalculateDurationMinutes(session.StartTime, session.EndTime, from, to)
                })
                .ToList();

            var categories = DefaultCategories
                .Select(categoryName =>
                {
                    var matchingSessions = sessions
                        .Where(session => NormalizeCategory(session.CategoryName).Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    var totalMinutes = matchingSessions.Sum(session => CalculateDurationMinutes(session.StartTime, session.EndTime, from, to));
                    var goalMinutes = matchingSessions.Select(session => session.DailyGoalMinutes).FirstOrDefault(goal => goal > 0);

                    return new CategoryTimeSummary
                    {
                        CategoryName = categoryName,
                        TotalMinutes = totalMinutes,
                        DailyGoalMinutes = goalMinutes,
                        GoalProgressPercent = goalMinutes > 0 ? Math.Min(100, totalMinutes / goalMinutes * 100) : 0,
                        IsGoalExceeded = goalMinutes > 0 && totalMinutes > goalMinutes
                    };
                })
                .ToList();

            var appTotals = sessions
                .GroupBy(session => string.IsNullOrWhiteSpace(session.AppName) ? "Unknown app" : session.AppName)
                .Select(group => new AppTimeSummary
                {
                    AppName = group.Key,
                    TotalMinutes = group.Sum(session => CalculateDurationMinutes(session.StartTime, session.EndTime, from, to))
                })
                .OrderByDescending(summary => summary.TotalMinutes)
                .Take(5)
                .ToList();

            var totalMinutes = categories.Sum(category => category.TotalMinutes);

            return new DashboardReport
            {
                ReportDate = from,
                TotalMinutes = totalMinutes,
                ProductiveMinutes = categories.First(category => category.CategoryName == "Productive").TotalMinutes,
                NeutralMinutes = categories.First(category => category.CategoryName == "Neutral").TotalMinutes,
                DistractingMinutes = categories.First(category => category.CategoryName == "Distracting").TotalMinutes,
                SessionCount = sessions.Count,
                MostUsedApp = appTotals.FirstOrDefault()?.AppName ?? "No sessions",
                LongestSessionMinutes = recentSessions.Count == 0 ? 0 : recentSessions.Max(session => session.DurationMinutes),
                Categories = categories,
                TopApplications = appTotals,
                RecentSessions = recentSessions.Take(10).ToList()
            };
        }

        private static string NormalizeCategory(string? categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return "Neutral";
            }

            return DefaultCategories.FirstOrDefault(category =>
                category.Equals(categoryName.Trim(), StringComparison.OrdinalIgnoreCase)) ?? "Neutral";
        }

        private static double CalculateDurationMinutes(DateTime startTime, DateTime endTime, DateTime from, DateTime to)
        {
            var effectiveStart = startTime < from ? from : startTime;
            var effectiveEnd = endTime > to ? to : endTime;

            if (effectiveEnd <= effectiveStart)
            {
                return 0;
            }

            return (effectiveEnd - effectiveStart).TotalMinutes;
        }
    }
}
