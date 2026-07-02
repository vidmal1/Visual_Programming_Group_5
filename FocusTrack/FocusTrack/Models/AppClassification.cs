namespace FocusTrack.Models
{
    public class AppClassification
    {
        public int Id { get; set; }
        public string AppName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public AppCategory Category { get; set; } = null!;
    }
}

