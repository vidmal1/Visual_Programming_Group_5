using System.ComponentModel.DataAnnotations;

namespace FocusTrack.Data.Entities
{
    public class IgnoreListItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string ApplicationName { get; set; } = string.Empty;
    }
}