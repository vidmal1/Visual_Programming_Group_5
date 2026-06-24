using FocusTrack.Core.Models;
using FocusTrack.Core.Native;
using System.Diagnostics;
using System.Text;

namespace FocusTrack.Business.Services
{
    public class TrackingService
    {
        public ForegroundWindowInfo GetCurrentForegroundWindow()
        {
            IntPtr handle = NativeMethods.GetForegroundWindow();

            StringBuilder windowTitleBuilder = new StringBuilder(500);
            NativeMethods.GetWindowText(handle, windowTitleBuilder, windowTitleBuilder.Capacity);

            string windowTitle = windowTitleBuilder.ToString();

            NativeMethods.GetWindowThreadProcessId(handle, out uint processId);

            string applicationName = "Unknown";

            try
            {
                Process process = Process.GetProcessById((int)processId);
                applicationName = process.ProcessName;
            }
            catch
            {
                applicationName = "Unknown";
            }

            return new ForegroundWindowInfo
            {
                ApplicationName = applicationName,
                WindowTitle = windowTitle,
                DetectedAt = DateTime.Now
            };
        }
    }
}