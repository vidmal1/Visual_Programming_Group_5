using System;

namespace FocusTrack.Models
{
    public class AppSession
    {
        
        public int Id { get; set; }

        public string AppName { get; set; }
        public string WindowTitle { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

       
        public int CategoryId { get; set; }
        public AppCategory Category { get; set; }
    }
}