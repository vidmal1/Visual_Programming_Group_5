using System.ComponentModel.DataAnnotations;

namespace FocusTrack.Data.Entities
{
    public class AppSession
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string ApplicationName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string WindowTitle { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int DurationSeconds { get; set; }

        public int CategoryId { get; set; } = 2;

        public Category Category { get; set; } = null!;
    }
}