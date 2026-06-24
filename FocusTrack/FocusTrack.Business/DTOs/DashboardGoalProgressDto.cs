namespace FocusTrack.Business.DTOs
{
    public class DashboardGoalProgressDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public int GoalMinutes { get; set; }

        public int UsedSeconds { get; set; }

        public string UsedText { get; set; } = string.Empty;

        public string RemainingText { get; set; } = string.Empty;

        public int ProgressPercentage { get; set; }
    }
}