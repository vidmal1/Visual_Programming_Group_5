using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FocusTrack.Core.Native
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCount
        );

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(
            IntPtr hWnd,
            out uint processId
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO lastInputInfo);

        public static int GetIdleTimeSeconds()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO));

            if (!GetLastInputInfo(ref lastInputInfo))
            {
                return 0;
            }

            uint idleMilliseconds = ((uint)Environment.TickCount) - lastInputInfo.dwTime;

            return (int)(idleMilliseconds / 1000);
        }
    }
}