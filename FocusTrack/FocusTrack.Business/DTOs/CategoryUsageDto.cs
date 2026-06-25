namespace FocusTrack.Business.DTOs
{
    public class CategoryUsageDto
    {
        public string CategoryName { get; set; } = string.Empty;

        public int SessionCount { get; set; }

        public int TotalSeconds { get; set; }

        public string DurationText { get; set; } = string.Empty;
    }
}