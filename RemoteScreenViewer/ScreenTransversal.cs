using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RemoteScreenViewer
{
    class ScreenTransversal
    {
        public static MemoryStream MS = null;
        /*屏幕截图*/
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest, //目标设备的句柄 
        int nXDest, // 目标对象的左上角的X坐标 
        int nYDest, // 目标对象的左上角的X坐标 
        int nWidth, // 目标对象的矩形的宽度 
        int nHeight, // 目标对象的矩形的长度 
        IntPtr hdcSrc, // 源设备的句柄 
        int nXSrc, // 源对象的左上角的X坐标 
        int nYSrc, // 源对象的左上角的X坐标 
        System.Int32 dwRop);// 光栅的操作值
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll",SetLastError=true)]
        private static extern IntPtr CreateDC(
        string lpszDriver, // 驱动名称 
        string lpszDevice, // 设备名称 
        string lpszOutput, // 无用，可以设定位"NULL" 
        IntPtr lpInitData); // 任意的打印机数据 
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);
        private Bitmap MyImage = null;

        //截图操作
        public byte[] ScreenJpeg()
        {
            IntPtr dc1 = CreateDC("DISPLAY", null, null, (IntPtr)null);
            if (dc1 == null)
            {
                int errCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                string msg = null;
                IntPtr tempptr = IntPtr.Zero;
                FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
                FormClient.Log("创建屏幕DC错误，错误码："+errCode+"\r\n"+msg);
                return null;
            }
            //创建显示器的DC 
            Graphics g1 = Graphics.FromHdc(dc1);
            //由一个指定设备的句柄创建一个新的Graphics对象 
            MyImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1);
            //根据屏幕大小创建一个与之相同大小的Bitmap对象 
            Graphics g2 = Graphics.FromImage(MyImage);
            //获得屏幕的句柄 
            IntPtr dc3 = g1.GetHdc();
            //获得位图的句柄 
            IntPtr dc2 = g2.GetHdc();
            //把当前屏幕捕获到位图对象中 
            BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376);
            //把当前屏幕拷贝到位图中 
            g1.ReleaseHdc(dc3);
            //释放屏幕句柄 
            g2.ReleaseHdc(dc2);
            //释放位图句柄 
            DeleteDC(dc1);
            Cursors.Arrow.Draw(g2, new Rectangle(Cursor.Position.X, Cursor.Position.Y, Cursors.Arrow.Size.Width, Cursors.Arrow.Size.Height));
            MS = new MemoryStream();
            MyImage.Save(MS, ImageFormat.Jpeg);
            return MS.GetBuffer();
        }
    }
}
