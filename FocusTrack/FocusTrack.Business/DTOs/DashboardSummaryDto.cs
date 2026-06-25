namespace FocusTrack.Business.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalSeconds { get; set; }

        public string TotalDurationText { get; set; } = string.Empty;

        public int SessionCount { get; set; }

        public string MostUsedApplication { get; set; } = "N/A";

        public string MostUsedDurationText { get; set; } = "0s";

        public int ProductivityScore { get; set; }

        public int ProductivePercentage { get; set; }

        public int NeutralPercentage { get; set; }

        public int DistractingPercentage { get; set; }

        public List<AppUsageDto> AppUsages { get; set; } = new List<AppUsageDto>();

        public List<CategoryUsageDto> CategoryUsages { get; set; } = new List<CategoryUsageDto>();

        public List<DashboardGoalProgressDto> GoalProgresses { get; set; } = new List<DashboardGoalProgressDto>();
    }
}