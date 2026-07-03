using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using FocusTrack.Helpers;
using FocusTrack.Models;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace FocusTrack.Services
{
    public class TrackingService : IDisposable
    {
        private readonly WinFormsTimer trackingTimer;
        private string currentApplicationName = string.Empty;
        private string currentWindowTitle = string.Empty;
        private DateTime sessionStartTime;
        private bool isTracking;

        public string CurrentAppName { get; private set; } = string.Empty;

        public TrackingService()
        {
            trackingTimer = new WinFormsTimer();
            trackingTimer.Interval = 5000;
            trackingTimer.Tick += TrackingTimer_Tick;
        }

        // Starts the timer and begins tracking the active application.
        public void StartTracking()
        {
            if (isTracking)
            {
                return;
            }

            isTracking = true;
            CaptureActiveWindow();
            trackingTimer.Start();
        }

        // Stops tracking and saves the current session if one is active.
        public void StopTracking()
        {
            if (!isTracking)
            {
                return;
            }

            trackingTimer.Stop();
            SaveCurrentSession();
            isTracking = false;
            currentApplicationName = string.Empty;
            currentWindowTitle = string.Empty;
        }

        // Runs every 5 seconds to check whether the active application has changed.
        private void TrackingTimer_Tick(object? sender, EventArgs e)
        {
            CaptureActiveWindow();
        }

        // Reads the foreground window, gets the app name and window title, and saves the session when the app changes.
        private void CaptureActiveWindow()
        {
            var windowHandle = GetForegroundWindow();
            if (windowHandle == IntPtr.Zero)
            {
                return;
            }

            var windowTitle = GetActiveWindowTitle(windowHandle);
            var applicationName = GetActiveApplicationName(windowHandle);

            if (string.IsNullOrWhiteSpace(applicationName))
            {
                return;
            }

            // Discard session if the application is in the ignore list
            if (StorageHelper.IsIgnoredApp(applicationName))
            {
                if (!string.IsNullOrEmpty(currentApplicationName))
                {
                    SaveCurrentSession();
                    currentApplicationName = string.Empty;
                    currentWindowTitle = string.Empty;
                }
                return;
            }

            if (string.IsNullOrWhiteSpace(currentApplicationName))
            {
                currentApplicationName = applicationName;
                currentWindowTitle = windowTitle;
                sessionStartTime = DateTime.Now;
                CurrentAppName = applicationName;
                return;
            }

            if (!string.Equals(currentApplicationName, applicationName, StringComparison.OrdinalIgnoreCase))
            {
                SaveCurrentSession();
                currentApplicationName = applicationName;
                currentWindowTitle = windowTitle;
                sessionStartTime = DateTime.Now;
                CurrentAppName = applicationName;
                return;
            }

            currentWindowTitle = windowTitle;
        }

        // Saves the current tracking session using StorageHelper.
        private void SaveCurrentSession()
        {
            if (string.IsNullOrWhiteSpace(currentApplicationName))
            {
                return;
            }

            var session = new AppSession
            {
                AppName = currentApplicationName,
                WindowTitle = currentWindowTitle,
                StartTime = sessionStartTime,
                EndTime = DateTime.Now,
                CategoryId = StorageHelper.ResolveCategoryId(currentApplicationName)
            };

            try
            {
                StorageHelper.SaveSession(session);
            }
            catch
            {
                // Keep tracking simple and avoid crashing the app if saving fails.
            }
        }

        // Gets the title of the active window.
        private static string GetActiveWindowTitle(IntPtr windowHandle)
        {
            var text = new StringBuilder(256);
            _ = GetWindowText(windowHandle, text, text.Capacity);
            return text.ToString();
        }

        // Gets the process name for the active window.
        private static string GetActiveApplicationName(IntPtr windowHandle)
        {
            GetWindowThreadProcessId(windowHandle, out var processId);

            if (processId == 0)
            {
                return string.Empty;
            }

            try
            {
                using var process = Process.GetProcessById((int)processId);
                return process.ProcessName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Dispose()
        {
            trackingTimer.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}