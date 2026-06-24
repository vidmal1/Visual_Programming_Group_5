namespace FocusTrack.Business.DTOs
{
    public class DailyGoalDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int GoalMinutes { get; set; }
    }
}