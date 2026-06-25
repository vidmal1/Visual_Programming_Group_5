namespace FocusTrack.Business.DTOs
{
    public class GoalAlertDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int GoalMinutes { get; set; }

        public string UsedText { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}