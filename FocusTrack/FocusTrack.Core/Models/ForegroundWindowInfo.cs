namespace FocusTrack.Core.Models
{
    public class ForegroundWindowInfo
    {
        public string ApplicationName { get; set; } = string.Empty;

        public string WindowTitle { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; } = DateTime.Now;

        public bool IsIgnored { get; set; }

        public bool IsIdle { get; set; }

        public int IdleSeconds { get; set; }
    }
}