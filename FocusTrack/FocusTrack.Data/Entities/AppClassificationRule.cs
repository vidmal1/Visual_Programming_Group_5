using System.ComponentModel.DataAnnotations;

namespace FocusTrack.Data.Entities
{
    public class AppClassificationRule
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string ApplicationName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}