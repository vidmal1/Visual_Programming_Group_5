namespace FocusTrack.Services
{
    public sealed class DashboardReport
    {
        public DateTime ReportDate { get; set; }
        public double TotalMinutes { get; set; }
        public double ProductiveMinutes { get; set; }
        public double NeutralMinutes { get; set; }
        public double DistractingMinutes { get; set; }
        public int SessionCount { get; set; }
        public string MostUsedApp { get; set; } = "No sessions";
        public double LongestSessionMinutes { get; set; }
        public IReadOnlyList<CategoryTimeSummary> Categories { get; set; } = Array.Empty<CategoryTimeSummary>();
        public IReadOnlyList<AppTimeSummary> TopApplications { get; set; } = Array.Empty<AppTimeSummary>();
        public IReadOnlyList<RecentSessionSummary> RecentSessions { get; set; } = Array.Empty<RecentSessionSummary>();
    }

    public sealed class CategoryTimeSummary
    {
        public string CategoryName { get; set; } = string.Empty;
        public double TotalMinutes { get; set; }
        public int DailyGoalMinutes { get; set; }
        public double GoalProgressPercent { get; set; }
        public bool IsGoalExceeded { get; set; }
    }

    public sealed class AppTimeSummary
    {
        public string AppName { get; set; } = string.Empty;
        public double TotalMinutes { get; set; }
    }

    public sealed class RecentSessionSummary
    {
        public string AppName { get; set; } = string.Empty;
        public string WindowTitle { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double DurationMinutes { get; set; }
    }
}
