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
        /*��Ļ��ͼ*/
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
        IntPtr hdcDest, //Ŀ���豸�ľ�� 
        int nXDest, // Ŀ���������Ͻǵ�X���� 
        int nYDest, // Ŀ���������Ͻǵ�X���� 
        int nWidth, // Ŀ�����ľ��εĿ�� 
        int nHeight, // Ŀ�����ľ��εĳ��� 
        IntPtr hdcSrc, // Դ�豸�ľ�� 
        int nXSrc, // Դ��������Ͻǵ�X���� 
        int nYSrc, // Դ��������Ͻǵ�X���� 
        System.Int32 dwRop);// ��դ�Ĳ���ֵ
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll",SetLastError=true)]
        private static extern IntPtr CreateDC(
        string lpszDriver, // �������� 
        string lpszDevice, // �豸���� 
        string lpszOutput, // ���ã������趨λ"NULL" 
        IntPtr lpInitData); // ����Ĵ�ӡ������ 
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);
        private Bitmap MyImage = null;

        //��ͼ����
        public byte[] ScreenJpeg()
        {
            IntPtr dc1 = CreateDC("DISPLAY", null, null, (IntPtr)null);
            if (dc1 == null)
            {
                int errCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                string msg = null;
                IntPtr tempptr = IntPtr.Zero;
                FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
                FormClient.Log("������ĻDC���󣬴����룺"+errCode+"\r\n"+msg);
                return null;
            }
            //������ʾ����DC 
            Graphics g1 = Graphics.FromHdc(dc1);
            //��һ��ָ���豸�ľ������һ���µ�Graphics���� 
            MyImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, g1);
            //������Ļ��С����һ����֮��ͬ��С��Bitmap���� 
            Graphics g2 = Graphics.FromImage(MyImage);
            //�����Ļ�ľ�� 
            IntPtr dc3 = g1.GetHdc();
            //���λͼ�ľ�� 
            IntPtr dc2 = g2.GetHdc();
            //�ѵ�ǰ��Ļ����λͼ������ 
            BitBlt(dc2, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, dc3, 0, 0, 13369376);
            //�ѵ�ǰ��Ļ������λͼ�� 
            g1.ReleaseHdc(dc3);
            //�ͷ���Ļ��� 
            g2.ReleaseHdc(dc2);
            //�ͷ�λͼ��� 
            DeleteDC(dc1);
            Cursors.Arrow.Draw(g2, new Rectangle(Cursor.Position.X, Cursor.Position.Y, Cursors.Arrow.Size.Width, Cursors.Arrow.Size.Height));
            MS = new MemoryStream();
            MyImage.Save(MS, ImageFormat.Jpeg);
            return MS.GetBuffer();
        }
    }
}
