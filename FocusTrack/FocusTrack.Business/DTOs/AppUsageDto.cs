namespace FocusTrack.Business.DTOs
{
    public class AppUsageDto
    {
        public string ApplicationName { get; set; } = string.Empty;

        public int TotalSeconds { get; set; }

        public string DurationText { get; set; } = string.Empty;
    }
}