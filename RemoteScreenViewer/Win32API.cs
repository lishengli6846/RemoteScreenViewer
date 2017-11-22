using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CommonClass
{
    public class Win32API
    {
        #region Win32 API
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true,
             CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern long GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowA", SetLastError = true)]
        public static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);

        public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint Msg, uint wParam, uint lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)]string path, [MarshalAs(UnmanagedType.LPTStr)]StringBuilder shortPath, int shortPathLength);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 将信息写入Ini文件
        /// </summary>
        /// <param name="section">写入INI文件的某个小节名称（不区分大小写）</param>
        /// <param name="key">小节section下的某个项的键名</param>
        /// <param name="val">项key对应的value</param>
        /// <param name="filePath">ini文件的文件名（包括路径）</param>
        /// <returns>返回0表示写入失败，否则成功</returns>
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 从ini文件读取信息
        /// </summary>
        /// <param name="section">INI文件的某个小节名称（不区分大小写）</param>
        /// <param name="key">小节section下的某个项的键名</param>
        /// <param name="def">当指定信息没找到时返回def，可以为空</param>
        /// <param name="retVal">一个字符缓冲区，其大小由size指定，返回的内容存在这里</param>
        /// <param name="size">retVal的大小</param>
        /// <param name="filePath">ini文件的文件名（包括路径）</param>
        /// <returns>返回取得信息字符串的长度</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string defValue, StringBuilder retVal, int size, string filePath);

        public static string GetConfigFromIni(string section, string key, string defValue,string iniFile="Config.ini")
        {
            string config = Application.StartupPath + "\\" + iniFile;
            StringBuilder sb = new StringBuilder();
            Win32API.GetPrivateProfileString(section,key, defValue, sb, 2550, config);
            return sb.ToString();
        }

        public static void SetConfigToIni(string section, string key, string value,string iniFile="Config.ini")
        {
            string config = Application.StartupPath + "\\" + iniFile;
            WritePrivateProfileString(section, key, value, config);
        }
        ///// <summary>
        ///// ShellExecute(IntPtr.Zero, "Open", "C:/Program Files/TTPlayer/TTPlayer.exe", "", "", 1);
        ///// </summary>
        ///// <param name="hwnd"></param>
        ///// <param name="lpOperation"></param>
        ///// <param name="lpFile"></param>
        ///// <param name="lpParameters"></param>
        ///// <param name="lpDirectory"></param>
        ///// <param name="nShowCmd"></param>
        ///// <returns></returns>
        //[DllImport("shell32.dll", EntryPoint = "ShellExecute")]
        //public static extern int ShellExecute(
        //IntPtr hwnd,
        //string lpOperation,
        //string lpFile,
        //string lpParameters,
        //string lpDirectory,
        //int nShowCmd
        //);
        //[DllImport("kernel32.dll")]
        //public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId); 
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern Int32 SystemParametersInfo(Int32 uAction, Int32 uParam, ref RECT lpvParam, Int32 fuWinIni);
        public struct RECT
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
            public RECT(int l, int t, int r, int b)
            {
                Left = l; Top = t; Right = r; Bottom = b;
            }
        }

        public static int GetTaskBarHeight()
        {
            int taskbar_height = 30;
            IntPtr hWnd = FindWindow("Shell_TrayWnd", null);
            System.Drawing.Rectangle rc = new System.Drawing.Rectangle();
            try { GetWindowRect(hWnd, ref rc); taskbar_height = Screen.PrimaryScreen.Bounds.Height - rc.Y; }
            catch { }
            return taskbar_height;
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref System.Drawing.Rectangle lpRect);

        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 0x8;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int WS_EX_MDICHILD = 0x40;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOSIZE = 0x1;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CLOSE = 0x10;
        private const int WS_CHILD = 0x40000000;

        private const int SW_HIDE = 0; //{隐藏, 并且任务栏也没有最小化图标}
        private const int SW_SHOWNORMAL = 1; //{用最近的大小和位置显示, 激活}
        private const int SW_NORMAL = 1; //{同 SW_SHOWNORMAL}
        private const int SW_SHOWMINIMIZED = 2; //{最小化, 激活}
        private const int SW_SHOWMAXIMIZED = 3; //{最大化, 激活}
        private const int SW_MAXIMIZE = 3; //{同 SW_SHOWMAXIMIZED}
        private const int SW_SHOWNOACTIVATE = 4; //{用最近的大小和位置显示, 不激活}
        private const int SW_SHOW = 5; //{同 SW_SHOWNORMAL}
        private const int SW_MINIMIZE = 6; //{最小化, 不激活}
        private const int SW_SHOWMINNOACTIVE = 7; //{同 SW_MINIMIZE}
        private const int SW_SHOWNA = 8; //{同 SW_SHOWNOACTIVATE}
        private const int SW_RESTORE = 9; //{同 SW_SHOWNORMAL}
        private const int SW_SHOWDEFAULT = 10; //{同 SW_SHOWNORMAL}
        private const int SW_MAX = 10;
        //{同 SW_SHOWNORMAL}

        //const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        //const int PROCESS_VM_READ = 0x0010;
        //const int PROCESS_VM_WRITE = 0x0020;  
        #endregion Win32 API

    }
}
