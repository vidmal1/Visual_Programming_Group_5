namespace FocusTrack.Business.DTOs
{
    public class ReportRowDto
    {
        public string GroupName { get; set; } = string.Empty;

        public int SessionCount { get; set; }

        public int TotalSeconds { get; set; }

        public string TotalDurationText { get; set; } = string.Empty;
    }
}