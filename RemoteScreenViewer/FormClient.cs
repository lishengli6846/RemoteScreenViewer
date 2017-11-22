using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonClass;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using Microsoft.Win32;

namespace RemoteScreenViewer
{
    public delegate void Delegate();
    public delegate void Delegate<T>(T arg1);
    public delegate void Delegate<T1,T2>(T1 arg1,T2 arg2);
    public delegate void Delegate<T1,T2,T3>(T1 arg1,T2 arg2,T3 arg3);
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            连接控制端CToolStripMenuItem_Click(null, null); 
            StartDataThread();

            //添加开机自动启动
            RunWhenStart(true, Application.ProductName, Application.StartupPath + @"\RemoteScreenViewer.exe");
        }

        private void StartDataThread()
        {
            dataListener = new TcpListener(IPAddress.Any, dataRecievePort);
            try
            {
                dataListener.Server.ReceiveTimeout = 3000;
                dataListener.Start();
                dataConnectReceiveThread = new Thread(new ThreadStart(receiveDataThreadRun));
                dataConnectReceiveThread.Start();
                status.Text = "数据接收准备就绪";
            }
            catch (SocketException se)
            {
                status.Text = "数据接收准备失败！"+se.Message;
            }

        }

        private void 设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormSettings().Show();
        }

        private void 连接控制端CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(ConnectServerThreadRun)).Start();
        }

        private void ConnectServerThreadRun()
        { 
            string serverIP = Win32API.GetConfigFromIni("Settings", "ServerIP", "");
            if(serverIP=="")
            {
                SetStatus("控制端IP配置错误！");
                return;
            }
            try
            {
                controlClient = new TcpClient();
                IPAddress _ips = IPAddress.Parse(serverIP);
                controlClient.ReceiveTimeout = 3000;
                controlClient.Connect(_ips, serverPort);
                controlNetworkStream = controlClient.GetStream();
                controlStreamReader = new StreamReader(controlNetworkStream);
                controlStreamWriter = new StreamWriter(controlNetworkStream);
                controlThread = new Thread(new ThreadStart(xiaoxi));
                controlThread.Start();
                SetStatus("已连接控制端");
                curState = "空闲";
            }
            catch (Exception ex)
            {
                SetStatus("控制端连接失败！" + (ex.Message.Contains("积极拒绝") ? "（控制端未启动）" : ex.Message));
            }
        }

        #region 参数的声明
        private int serverPort = 1315;
        private int dataRecievePort = 1316;
        private string sendScreenToIP = "";
        private TcpClient controlClient = null, dataClient = null;
        private TcpListener dataListener = null;
        private Socket dataSocket = null;
        private NetworkStream controlNetworkStream = null, dataNetworkStream = null;
        private Thread controlThread = null;//接受消息的线程
        private Thread screenShotThread = null;
        private Thread dataConnectReceiveThread = null;
        private Thread dataScreenReceiveThread = null;
        private StreamReader controlStreamReader = null;//读取文本消息           
        private StreamWriter controlStreamWriter = null;//写入文本消息
        private string curState = "空闲";
        private Image MyImage=null;
        private bool IsRunning = true;
        ScreenTransversal ST = new ScreenTransversal();
        #endregion

        #region 调用API
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public extern static bool SetCursorPos(int x, int y);
        #endregion

        #region 连接到服务器
        public void ShowSend()
        {
            try
            {
                controlClient = new TcpClient();
                IPAddress _ips = Dns.GetHostAddresses(Dns.GetHostName())[0];//获取本的IP地址
                controlClient.Connect(_ips, serverPort);
                controlNetworkStream = controlClient.GetStream();
                controlStreamReader = new StreamReader(controlNetworkStream);
                controlStreamWriter = new StreamWriter(controlNetworkStream);
                controlThread = new Thread(new ThreadStart(xiaoxi));
                controlThread.Start();
            }
            catch (Exception ex)
            {
                ShowSend();
            }
        }
        #endregion


        #region 接受图片
        //接受图片
        public void xianshi()
        {
            while (IsRunning)
            {
                try
                {
                    byte[] length = new byte[4];
                    dataSocket.Receive(length, 0, 4, SocketFlags.None);
                    int len = BitConverter.ToInt32(length, 0);
                    byte[] by = new byte[len];
                    int received=0;
                    DateTime t1 = DateTime.Now;
                    while (received < len && (DateTime.Now-t1).TotalSeconds<5)  //设置5秒超时
                    {
                        received += dataSocket.Receive(by, received, len - received, SocketFlags.None);
                    }
                    //if (received == 0) { Thread.Sleep(100); continue; }
                    if (received != len)
                        throw new Exception("数据长度不足");
                    MyImage = Image.FromStream(new MemoryStream(by));
                    //pictureBox1.Image = MyImage;//显示图片
                    SetImage(MyImage);
                    curState = "显示："+(dataSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("远程主机强迫关闭了一个现有的连接"))
                    //{
                    //    //pictureBox1.Image = null;
                    //    SetImage(null);
                    //    //break;
                    //}
                    //Log(ex.ToString());

                    SetStatus("接受图片异常:" + ex.Message);
                    SetImage(null);
                    //HideWindow();
                    curState = "空闲";
                    break;
                }
            }
        }
        #endregion

        private void SetImage(Image img)
        {
            if (pictureBox1.InvokeRequired)
                pictureBox1.Invoke(new Delegate<Image>(SetImage), img);
            else
                pictureBox1.Image = img;
        }

        //接受数据
        #region 接受数据连接线程
        public void receiveDataThreadRun()
        {
            while (IsRunning)
            {
                try
                {
                    dataSocket = dataListener.AcceptSocket();
                    dataSocket.ReceiveTimeout = 3000;
                    string ip = (dataSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                    SetText(this, "远程屏幕监控  -  " + ip);
                    if (dataScreenReceiveThread != null)    //关闭之前的数据线程
                        dataScreenReceiveThread.Abort();//---------------不知是否能够关闭

                    dataScreenReceiveThread = new Thread(new ThreadStart(xianshi)); //重开数据线程
                    dataScreenReceiveThread.Start();
                }
                catch (Exception e) { Log(e.ToString()); }
            }
        }
        #endregion

        //接受消息
        #region 接受消息作出相应的事件信息
        public void xiaoxi()
        {
            try
            {
                string ling;
                while (IsRunning)
                {
                    ling = controlStreamReader.ReadLine();
                    if (ling != null)
                    {
                        string[] arr = ling.Split('：');
                        if (arr[0].Equals("jietu"))
                        {
                            screenShotThread = new Thread(new ThreadStart(jietu));
                            screenShotThread.Start();
                        }
                        if (arr[0].Equals("mouset"))
                        {
                            int x = Convert.ToInt32(arr[1]);
                            int y = Convert.ToInt32(arr[2]);
                            MessageBox.Show(x + "," + y);
                            SetCursorPos(x, y);

                        }
                        if (arr[0].Equals("fasong"))
                        {
                            sendScreenToIP = arr[1];
                            if (screenShotThread != null) screenShotThread.Abort();
                            curState = "空闲";
                            screenShotThread = new Thread(new ThreadStart(jietu));
                            screenShotThread.Start();
                            HideWindow();
                        }
                        if (arr[0].Equals("jieshou"))
                        {
                            setWindowMode(arr[1]);
                            sendScreenToIP = "";
                        }
                        if (arr[0].Equals("tingzhifasong"))
                        {
                            sendScreenToIP = "";
                            if (screenShotThread != null) screenShotThread.Abort();
                            curState = "空闲";
                            //setWindowMode("chuangkou");
                            HideWindow();
                        }
                        if (arr[0].Equals("tingzhijieshou"))
                        {
                            sendScreenToIP = "";
                            pictureBox1.Image = null;
                            //setWindowMode("chuangkou");
                            HideWindow();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                controlClient.Client.Close();
                controlThread.Abort();
                Log(ex.ToString());
                //if (screenShotThread != null)
                //    screenShotThread.Abort();
                //ShowSend();
            }
        }
        #endregion

        private void HideWindow()
        {
            if (this.InvokeRequired)
                this.Invoke(new Delegate(HideWindow));
            else
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
                //this.notifyIcon1.Visible = true;
                //this.WindowState = FormWindowState.Minimized;
            }
        }
        private void SetText(Control c, string text)
        {
            if (c.InvokeRequired)
                c.Invoke(new Delegate<Control, string>(SetText), c, text);
            else
                c.Text = text;
        }
        private void SetStatus(string info)
        {
            if (this.InvokeRequired)
                this.Invoke(new Delegate<string>(SetStatus), info);
            else
                status.Text = info;
        }
        private void setWindowMode(string mode)
        {
            if (this.InvokeRequired)
                this.Invoke(new Delegate<string>(setWindowMode), mode);
            else
            {
                bool isFullScreen = mode == "quanping";
                this.TopMost = isFullScreen;
                this.FormBorderStyle = isFullScreen ? FormBorderStyle.None : System.Windows.Forms.FormBorderStyle.Sizable;
                //this.WindowState = isFullScreen ? FormWindowState.Maximized : FormWindowState.Normal;
                this.Location = new Point(0, 0);
                this.Size = isFullScreen ? Screen.PrimaryScreen.Bounds.Size : new Size(439, 377);
                this.menuStrip1.Visible = !isFullScreen;
                this.statusStrip1.Visible = !isFullScreen;
                this.ShowInTaskbar = true;
                this.Visible = true;
                this.BringToFront();
            }
        }

        //截图程序
        #region 截取图片并发送
        public void jietu()
        {
            try
            {
                dataClient = new TcpClient();
                IPAddress _ips = IPAddress.Parse(sendScreenToIP);
                dataClient.Connect(_ips, dataRecievePort);
                dataNetworkStream = dataClient.GetStream();
                //dataStreamReader = new StreamReader(controlNetworkStream);
                //controlStreamWriter = new StreamWriter(controlNetworkStream);

                while (IsRunning)
                {
                    try
                    {
                        byte[] by = null;
                        try
                        {
                            by = ST.ScreenJpeg();
                        }
                        catch (ArgumentException ae) { Log(ae.ToString()); }
                        if (by != null)
                        {
                            dataNetworkStream.Write(ConvertToByteArray(by.Length), 0, 4);   //先写入4个字节的图片长度信息
                            dataNetworkStream.Write(by, 0, by.Length);
                            //dataNetworkStream.Flush();
                            //ScreenTransversal.MS.Flush();
                            curState = "发送至：" + _ips.ToString();
                        }
                        Thread.Sleep(40);
                    }
                    catch (Exception ex)
                    {
                        //screenShotThread.Abort();
                        dataNetworkStream.Close();
                        dataClient.Client.Close();
                        curState = "空闲";
                        //MessageBox.Show("发送数据错误，点击确定重连\r\n"+ex.ToString());
                        Log(ex.ToString());
                        //如果sendScreenToIP仍指示有发送地址，且与当前所用地址相同，尝试自动重连
                        if (sendScreenToIP != "" && sendScreenToIP == _ips.Address.ToString())
                        {
                            try
                            {
                                Thread.Sleep(100);
                                Log("开始重连");
                                dataClient = new TcpClient();
                                dataClient.Connect(_ips, dataRecievePort);
                                dataNetworkStream = dataClient.GetStream();
                            }
                            catch (Exception e2) { Log(e2.ToString()); }
                        }
                        else
                            break;
                    }
                }
            }
            catch (Exception ee1) { Log(ee1.ToString()); }
        }

        public static void Log(string log)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory+"\\log" + DateTime.Now.ToString("yyyyMMdd") + ".log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("----------------------------------------------------------------------------\r\nError-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    " + log);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void RunWhenStart(bool isStart, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (isStart)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch (Exception e) { }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch (Exception e1) { }
            }
        }

        public static bool IsRunWhenStart(string name)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            foreach (string s in Run.GetValueNames())
                if (s == name)
                    return true;
            return false;
        }

        static byte[] ConvertToByteArray(int i)
        {
            byte[] arry = new byte[4];
            arry[0] = (byte)(i & 0xFF);
            arry[1] = (byte)((i & 0xFF00) >> 8);
            arry[2] = (byte)((i & 0xFF0000) >> 16);
            arry[3] = (byte)((i >> 24) & 0xFF);
            return arry;
        }
        #endregion

        #region 按钮事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRunning)
            {
                HideWindow();
                e.Cancel = true;
            }
            else
            {
                try
                {
                    TcpClient c = new TcpClient();
                    c.Connect(IPAddress.Parse("127.0.0.1"), dataRecievePort);
                    c.Close();
                }
                catch { }
                if (controlThread != null)
                {
                    controlThread.Abort();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //写入数据测试连接是否有效
            try
            {
                if (controlNetworkStream != null)
                {
                    controlStreamWriter.WriteLine("状态："+curState);
                    controlStreamWriter.Flush();
                }
                else
                    连接控制端CToolStripMenuItem_Click(null, null);
            }
            catch(IOException ioe) 
            {
                status.Text = "网络断开，正在重连...";
                curState = "空闲";
                连接控制端CToolStripMenuItem_Click(null, null);
            }
        }

        private void notifyIcon1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.ShowInTaskbar)
                {
                    this.ShowInTaskbar = false;
                    this.Visible = false;
                    this.TopMost = false;
                    //this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else
                {
                    this.ShowInTaskbar = true;
                    this.Visible = true;
                    this.TopMost = true;
                    //this.WindowState = FormWindowState.Normal;
                    this.Show();
                }
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsRunning = false;
            this.Close();
            Application.Exit();
        }

    }
}
