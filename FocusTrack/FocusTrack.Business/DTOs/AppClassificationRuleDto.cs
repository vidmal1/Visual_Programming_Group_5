namespace FocusTrack.Business.DTOs
{
    public class AppClassificationRuleDto
    {
        public int Id { get; set; }

        public string ApplicationName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}