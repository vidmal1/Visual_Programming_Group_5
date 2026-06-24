namespace FocusTrack.Data.Entities
{
    public class DailyGoal
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int GoalMinutes { get; set; }
    }
}