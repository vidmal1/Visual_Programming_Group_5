using System.Collections.Generic;

namespace FocusTrack.Models
{
    public class AppCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DailyGoalMinutes { get; set; }
        public ICollection<AppSession> Sessions { get; set; } = new List<AppSession>();
    }
}
