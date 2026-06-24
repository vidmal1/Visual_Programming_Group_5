using System.ComponentModel.DataAnnotations;

namespace FocusTrack.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ColorHex { get; set; } = "#6B7280";

        public ICollection<AppSession> AppSessions { get; set; } = new List<AppSession>();

        public ICollection<DailyGoal> DailyGoals { get; set; } = new List<DailyGoal>();
    }
}