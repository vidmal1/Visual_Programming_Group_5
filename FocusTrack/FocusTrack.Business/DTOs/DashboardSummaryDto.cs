namespace FocusTrack.Business.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalSeconds { get; set; }

        public string TotalDurationText { get; set; } = string.Empty;

        public int SessionCount { get; set; }

        public string MostUsedApplication { get; set; } = "N/A";

        public string MostUsedDurationText { get; set; } = "0s";

        public List<AppUsageDto> AppUsages { get; set; } = new List<AppUsageDto>();
    }
}