using System;
using System.Collections.Generic;
using System.Text;

namespace FocusTrack.Core.Models
{
    public class ForegroundWindowInfo
    {
        public string ApplicationName { get; set; } = string.Empty;

        public string WindowTitle { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; } = DateTime.Now;
    }
}
