using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace RemoteScreenViewerController
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormServer());
            }
            else
            {
                MessageBox.Show("屏幕监控管理已在运行，不能重复运行！\r\n请先退出之前的程序或用任务管理器结束RemoteScreenViewerController.exe");
            }
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);//=========================
            foreach (Process p in processes)
            {
                if (p.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return p;
                    }
                }
            }
            return null;
        }
    }
}
