namespace FocusTrack.Data
{
    public sealed class DashboardSessionRecord
    {
        public int Id { get; set; }
        public string AppName { get; set; } = string.Empty;
        public string WindowTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CategoryName { get; set; } = "Neutral";
        public int DailyGoalMinutes { get; set; }
    }
}
