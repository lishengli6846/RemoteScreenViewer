using RemoteScreenViewer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CommonClass;

namespace RemoteScreenViewerController
{
    public partial class FormServer : Form
    {
        public FormServer()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            StartUp();
        }
        
        #region 参数的申明
        private int _port = 1315;
        private TcpListener _tcpl = null;
        private Hashtable _transmit_tb = new Hashtable();
        private Image MyImage = null;
        private Socket newClient = null;
        private Thread td = null;
        private Thread threadReceiveConnections = null;
        private bool IsRunning = true;
        private Thread shubiao = null;
        private NetworkStream networkstream = null;
        private StreamWriter streamWriter = null;
        private const string iniFile = "ServerConfig.ini";
        private string previewMode = "quanping";  //quanping  chuangkou
        private ClientInfo DefaultShowDevice = null;
        #endregion

        #region 调用API
        ///<summary>
        ///获取鼠标的坐标
        ///</summary>
        ///<param name="lpPoint"></param>
        ///<returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public extern static bool GetCursorPos(out MousePositions lpPoint);
        #endregion

        #region 开启服务器
        public void StartUp()
        {
            //IPAddress _ip = Dns.GetHostAddresses(Dns.GetHostName())[0];
            _tcpl = new TcpListener(IPAddress.Any, _port);
            try
            {
                _tcpl.Server.ReceiveTimeout = 3000;
                _tcpl.Start();
                threadReceiveConnections = new Thread(new ThreadStart(ReceiveConnectionsThreadRun));
                threadReceiveConnections.Start();
                MessageBox.Show("服务端已启动", "系统消息");
            }
            catch (SocketException se)
            {
                MessageBox.Show("控制端启动失败，"+_port+"端口可能被占用！\r\n"+se.Message);
            }
        }
        public void ReceiveConnectionsThreadRun()
        {
            while (IsRunning)
            {
                newClient = _tcpl.AcceptSocket();
                string ip = (newClient.RemoteEndPoint as IPEndPoint).Address.ToString();
                string name = Win32API.GetConfigFromIni("Names", ip, "", iniFile);
                ClientInfo ci = new ClientInfo(ip,name,newClient);
                //_transmit_tb.Add(ip, ci);
                lock (listBoxUsable)
                {
                    for (int i = listBoxUsable.Items.Count - 1; i >= 0; i--)    //删除重复记录
                        if ((listBoxUsable.Items[i] as ClientInfo).IP == ip)
                        {
                            try
                            {
                                ClientInfo ciRemoved = listBoxUsable.Items[i] as ClientInfo;
                                listBoxUsable.Items.RemoveAt(i);
                                ciRemoved.Socket.Close();
                            }
                            catch (Exception e) { FormClient.Log(e.ToString()); }
                        }
                }
                listBoxUsable.Items.Add(ci);
                //networkstream = new NetworkStream(newClient);
                //streamWriter = new StreamWriter(networkstream);
                if (newClient.Connected)    //对每个客户端开启一个连接
                {
                    newClient.ReceiveTimeout = 3000;
                    Thread receiveControlMessageThread = new Thread(new ParameterizedThreadStart(receiveControlMessageThreadRun));
                    receiveControlMessageThread.Start(newClient);
                }
            }
        }
        #endregion

        private void receiveControlMessageThreadRun(object socket)
        {
            try
            {
                string ling;
                NetworkStream ns = new NetworkStream(socket as Socket);
                string myIP = ((socket as Socket).RemoteEndPoint as IPEndPoint).Address.ToString();
                StreamReader sr = new StreamReader(ns as NetworkStream);
                while (IsRunning)
                {
                    ling = sr.ReadLine();
                    if (ling != null)
                    { 
                        string[] arr = ling.Split('：');
                        if (arr[0].Equals("状态"))
                        {
                            updateState(myIP, arr[1], arr.Length == 3 ? arr[2] : "");
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private void updateState(string myIP, string state,string remoteIP)
        {
            if (listBoxUsable.InvokeRequired)
                listBoxUsable.Invoke(new Delegate<string, string, string>(updateState), myIP, state, remoteIP);
            else
            {
                for (int i = 0; i < listBoxUsable.Items.Count; i++)
                {
                    ClientInfo ci = listBoxUsable.Items[i] as ClientInfo;
                    if (ci.IP == myIP)
                    {
                        string name = Win32API.GetConfigFromIni("Names", myIP, myIP, iniFile);
                        string rName = Win32API.GetConfigFromIni("Names", remoteIP, remoteIP, iniFile);
                        listBoxUsable.Items[i] = new ClientInfo(myIP, name, ci.Socket, state+" "+rName);
                        break;
                    }
                }
            }
        }

        #region 按钮事件
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //if (toolStripButton1.Text == "开启服务器")
            //{
            //    toolStripButton1.Text = "关闭服务器";
            //    StartUp();
            //    toolStripButton2_Click(sender, e);
            //}
            //else
            //{
            //    toolStripButton1.Text = "开启服务器";
            //    _tcpl.Stop();
            //    _tcpl.Server.Close();
            //    td.Abort();
            //    if (shubiao != null)
            //        shubiao.Abort();
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRunning)
            {
                HideWindow();
                e.Cancel = true;
            }
            else
            {
                if (td != null)
                    td.Abort();
                if (shubiao != null)
                    shubiao.Abort();
                IsRunning = false;
                try
                {
                    TcpClient c = new TcpClient();
                    c.Connect(IPAddress.Parse("127.0.0.1"), _port);
                    c.Close();
                }
                catch { }
                if (threadReceiveConnections != null)
                    threadReceiveConnections.Abort();
                Application.Exit();
            }
        }

        private void HideWindow()
        {
            if (this.InvokeRequired)
                this.Invoke(new RemoteScreenViewer.Delegate(HideWindow));
            else
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
            }
        }

        private void ShowWindow()
        { 
            if (this.InvokeRequired)
                this.Invoke(new RemoteScreenViewer.Delegate(HideWindow));
            else
            {
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                this.Show();
                this.BringToFront();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Escape)//获取或设置按下的相应键
            //{
            //    this.FormBorderStyle = FormBorderStyle.Sizable;
            //    this.toolStrip1.Visible = true;
            //    this.WindowState = FormWindowState.Normal;
            //}
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            //this.toolStrip1.Visible = false;
            this.WindowState = FormWindowState.Maximized;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            shubiao = new Thread(new ThreadStart(mosue_send));
            shubiao.Start();
        }
        #endregion

        #region 发送鼠标坐标
        public void mosue_send()
        {
            try
            {
                MousePositions MP;
                while (true)
                {
                    GetCursorPos(out MP);
                    streamWriter.WriteLine("mouset：" + MP.x + "：" + MP.y);//发送坐标
                    streamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("鼠标异常" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 接受图片
        //接受图片
        //public void xianshi()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            byte[] by = new byte[1024 * 256*10];
        //            newClient.Receive(by);
        //            MyImage = Image.FromStream(new MemoryStream(by));
        //            //pictureBox1.Image = MyImage;//显示图片
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("接受图片异常" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
        #endregion

        private void listBoxUsable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (listBoxUsable.SelectedItem == null)
                {
                    预览PToolStripMenuItem.Enabled = false;
                    发送至SToolStripMenuItem.Enabled = false;
                    备注NToolStripMenuItem.Enabled = false;
                    设为默认显示ToolStripMenuItem.Enabled = false;
                    断开重连CToolStripMenuItem.Enabled = false;
                }
                else
                {
                    预览PToolStripMenuItem.Enabled = true;
                    发送至SToolStripMenuItem.Enabled = true;
                    备注NToolStripMenuItem.Enabled = true;
                    断开重连CToolStripMenuItem.Enabled = true;
                    设为默认显示ToolStripMenuItem.Enabled = true;
                    设为默认显示ToolStripMenuItem.Text = (DefaultShowDevice!=null && (listBoxUsable.SelectedItem as ClientInfo).IP == DefaultShowDevice.IP) ? "取消默认显示" : "设为默认显示";
                    发送至SToolStripMenuItem.DropDownItems.Clear();
                    List<string> items = new List<string>();
                    foreach (ClientInfo ci in listBoxUsable.Items)
                    {
                        if (ci.ToString() != listBoxUsable.SelectedItem.ToString())
                            items.Add(ci.ToString());
                    }
                    items.Sort();
                    foreach(string i  in items)
                    {
                        ToolStripItem item = 发送至SToolStripMenuItem.DropDownItems.Add(i);
                        item.Click += clientItem_Click;
                    }
                }
            }
        }

        //发送至。。。
        void clientItem_Click(object sender, EventArgs e)
        {
            //获取要建立的两个Client
            ClientInfo srcClientInfo = listBoxUsable.SelectedItem as ClientInfo;
            if (srcClientInfo == null) return;
            ToolStripItem item = sender as ToolStripItem;
            ClientInfo destClientInfo = null;
            foreach (ClientInfo ci in listBoxUsable.Items)
                if (ci.ToString() == item.Text)
                    destClientInfo = ci;
            if (destClientInfo == null) return;
            establishScreenView(srcClientInfo, destClientInfo, "quanping");
        }

        private void establishScreenView(ClientInfo srcClientInfo, ClientInfo destClientInfo, string previewMode)
        { 
            //断开与即将建立的连接有冲突的
            for (int i = listBoxEstablished.Items.Count - 1; i >= 0; i--)
            {
                ScreenViewInfo info = listBoxEstablished.Items[i] as ScreenViewInfo;
                if (info.srcClient.IP == srcClientInfo.IP
                    || info.srcClient.IP == destClientInfo.IP
                    || info.destClient.IP == srcClientInfo.IP
                    || info.destClient.IP == destClientInfo.IP)
                {
                    try
                    {
                        listBoxEstablished.Items.RemoveAt(i);
                        StreamWriter sw = new StreamWriter(new NetworkStream(info.srcClient.Socket));
                        sw.WriteLine("tingzhifasong");
                        sw.Flush();
                        sw = new StreamWriter(new NetworkStream(info.destClient.Socket));
                        sw.WriteLine("tingzhijieshou");
                        sw.Flush();
                    }
                    catch (Exception e1) { }
                }
            }
            //添加已建立连接列表并发送连接指令
            try
            {
                ScreenViewInfo svi = new ScreenViewInfo(srcClientInfo, destClientInfo);
                listBoxEstablished.Items.Add(svi);
                StreamWriter sw2 = new StreamWriter(new NetworkStream(srcClientInfo.Socket));
                sw2.WriteLine("fasong：" + destClientInfo.IP);
                sw2.Flush();
                sw2 = new StreamWriter(new NetworkStream(destClientInfo.Socket));
                sw2.WriteLine("jieshou：" + previewMode);
                sw2.Flush();
            }
            catch (Exception e2) { }
        }

        private void 预览PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获取要建立的两个Client
            ClientInfo srcClientInfo = listBoxUsable.SelectedItem as ClientInfo;
            if (srcClientInfo == null)
            {
                return;
            }
            ClientInfo destClientInfo = null;
            IPAddress[] iplist = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (ClientInfo ci in listBoxUsable.Items)
                foreach(IPAddress ip in iplist)
                    if (ip.ToString() == ci.IP)
                    {
                        destClientInfo = ci;
                        break;
                    }
            if (destClientInfo == null)
            {
                if (File.Exists(Application.StartupPath + "\\RemoteScreenViewer.exe"))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\RemoteScreenViewer.exe");
                    Thread.Sleep(100);
                    foreach (ClientInfo ci in listBoxUsable.Items)  //再次查找本机
                        foreach (IPAddress ip in iplist)
                            if (ip.ToString() == ci.IP)
                            {
                                destClientInfo = ci;
                                break;
                            }
                }
            }
            if (destClientInfo == null) return;
            establishScreenView(srcClientInfo, destClientInfo, "chuangkou");
        }

        private void 备注NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRename rename = new FormRename();
            if (rename.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ClientInfo c in listBoxUsable.Items)
                    if (c.Name == rename.name)
                    { MessageBox.Show("与当前列表中名称有重复！"); return; }
                ClientInfo ci = listBoxUsable.SelectedItem as ClientInfo;
                Win32API.SetConfigToIni("Names", ci.IP, ci.Name, iniFile);
                listBoxUsable.Items[listBoxUsable.SelectedIndex] = new ClientInfo(ci.IP, rename.name, ci.Socket, ci.State);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (listBoxUsable)
            {
                for (int i = listBoxUsable.Items.Count - 1; i >= 0; i--) // ClientInfo ci in listBoxUsable.Items)
                {
                    try
                    {
                        ClientInfo ci = listBoxUsable.Items[i] as ClientInfo;
                        StreamWriter sw = new StreamWriter(new NetworkStream(ci.Socket));
                        sw.WriteLine("alive");
                        sw.Flush();
                    }
                    catch (Exception e2)
                    {
                        if (DefaultShowDevice != null && (listBoxUsable.Items[i] as ClientInfo).IP == DefaultShowDevice.IP)
                        {
                            DefaultShowDevice = null;
                            status.Text = "";
                        }
                        try
                        {
                            ClientInfo ciRevoced = listBoxUsable.Items[i] as ClientInfo;
                            listBoxUsable.Items.RemoveAt(i);
                            ciRevoced.Socket.Close();
                        }
                        catch (Exception e3) { FormClient.Log(e3.ToString()); }
                    }
                }
            }
        }

        private void 断开SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenViewInfo info = listBoxEstablished.SelectedItem as ScreenViewInfo;
            if (info == null) return;
            try
            {
                listBoxEstablished.Items.Remove(listBoxEstablished.SelectedItem);
                StreamWriter sw = new StreamWriter(new NetworkStream(info.srcClient.Socket));
                sw.WriteLine("tingzhifasong");
                sw.Flush();
                sw = new StreamWriter(new NetworkStream(info.destClient.Socket));
                sw.WriteLine("tingzhijieshou");
                sw.Flush();
            }
            catch (Exception e1) { }

        }

        private void listBoxUsable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxUsable.SelectedItem == null) return;
            if (DefaultShowDevice != null)
            {
                ClientInfo srcClientInfo = listBoxUsable.SelectedItem as ClientInfo;
                establishScreenView(srcClientInfo, DefaultShowDevice, "quanping");
            }
            else 
                预览PToolStripMenuItem_Click(null, null);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.ShowInTaskbar) HideWindow();
                else ShowWindow();
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsRunning = false;
            this.Close();
            Application.Exit();
        }

        private void 设为默认显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (设为默认显示ToolStripMenuItem.Text == "设为默认显示")
                DefaultShowDevice = listBoxUsable.SelectedItem as ClientInfo;
            else
                DefaultShowDevice = null;
            status.Text = DefaultShowDevice == null ? "" : "当前默认显示端为:" + DefaultShowDevice.Name;
        }

        private void 断开重连CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientInfo ci = listBoxUsable.SelectedItem as ClientInfo;
            if (ci == null) return;
            try
            {
                listBoxUsable.Items.Remove(listBoxUsable.SelectedItem);
                ci.Socket.Close();
            }
            catch (Exception e1) { FormClient.Log(e1.ToString()); }
        }

    }
}
