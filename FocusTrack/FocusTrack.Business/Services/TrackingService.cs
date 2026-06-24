using FocusTrack.Core.Models;
using FocusTrack.Core.Native;
using FocusTrack.Data.Entities;
using FocusTrack.Data.Repositories;
using System.Diagnostics;
using System.Text;

namespace FocusTrack.Business.Services
{
    public class TrackingService
    {
        private readonly SessionRepository _sessionRepository = new SessionRepository();
        private readonly IgnoreListRepository _ignoreListRepository = new IgnoreListRepository();
        private AppSession? _currentSession;

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

        public async Task<ForegroundWindowInfo> TrackCurrentWindowAsync()
        {
            ForegroundWindowInfo windowInfo = GetCurrentForegroundWindow();

            if (string.IsNullOrWhiteSpace(windowInfo.WindowTitle))
            {
                return windowInfo;
            }

            bool isIgnored = await _ignoreListRepository.IsIgnoredAsync(windowInfo.ApplicationName);

            windowInfo.IsIgnored = isIgnored;

            if (isIgnored)
            {
                await EndCurrentSessionAsync();

                return windowInfo;
            }

            if (_currentSession == null)
            {
                await StartNewSessionAsync(windowInfo);
                return windowInfo;
            }

            bool isWindowChanged =
                _currentSession.ApplicationName != windowInfo.ApplicationName ||
                _currentSession.WindowTitle != windowInfo.WindowTitle;

            if (isWindowChanged)
            {
                await EndCurrentSessionAsync();
                await StartNewSessionAsync(windowInfo);
            }

            return windowInfo;
        }

        public async Task StopTrackingAsync()
        {
            await EndCurrentSessionAsync();
        }

        private async Task StartNewSessionAsync(ForegroundWindowInfo windowInfo)
        {
            AppSession session = new AppSession
            {
                ApplicationName = windowInfo.ApplicationName,
                WindowTitle = windowInfo.WindowTitle,
                StartTime = DateTime.Now,
                EndTime = null,
                DurationSeconds = 0,
                CategoryId = 2
            };

            await _sessionRepository.AddAsync(session);

            _currentSession = session;
        }

        private async Task EndCurrentSessionAsync()
        {
            if (_currentSession == null)
            {
                return;
            }

            DateTime endTime = DateTime.Now;

            await _sessionRepository.EndSessionAsync(_currentSession.Id, endTime);

            _currentSession = null;
        }
    }
}