using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonClass;

namespace RemoteScreenViewer
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            tbServerIP.Text = Win32API.GetConfigFromIni("Settings", "ServerIP", "");
            cbAutoStart.Checked = FormClient.IsRunWhenStart(Application.ProductName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Win32API.SetConfigToIni("Settings", "ServerIP", tbServerIP.Text);
            FormClient.RunWhenStart(cbAutoStart.Checked, Application.ProductName, Application.StartupPath + @"\RemoteScreenViewer.exe");
            this.Close();
        }
    }
}
