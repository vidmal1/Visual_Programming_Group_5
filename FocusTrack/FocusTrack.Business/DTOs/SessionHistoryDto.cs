namespace FocusTrack.Business.DTOs
{
    public class SessionHistoryDto
    {
        public int Id { get; set; }

        public string ApplicationName { get; set; } = string.Empty;

        public string WindowTitle { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int DurationSeconds { get; set; }

        public string DurationText { get; set; } = string.Empty;
    }
}