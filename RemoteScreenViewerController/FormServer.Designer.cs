namespace RemoteScreenViewerController
{
    partial class FormServer
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServer));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxUsable = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.备注NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.预览PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发送至SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设为默认显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxEstablished = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.断开SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.断开重连CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前可用机器列表：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "已建立的监控：";
            // 
            // listBoxUsable
            // 
            this.listBoxUsable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxUsable.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxUsable.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxUsable.FormattingEnabled = true;
            this.listBoxUsable.HorizontalScrollbar = true;
            this.listBoxUsable.ItemHeight = 14;
            this.listBoxUsable.Location = new System.Drawing.Point(24, 49);
            this.listBoxUsable.Name = "listBoxUsable";
            this.listBoxUsable.Size = new System.Drawing.Size(228, 340);
            this.listBoxUsable.TabIndex = 1;
            this.listBoxUsable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxUsable_MouseDoubleClick);
            this.listBoxUsable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxUsable_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.备注NToolStripMenuItem,
            this.预览PToolStripMenuItem,
            this.发送至SToolStripMenuItem,
            this.断开重连CToolStripMenuItem,
            this.设为默认显示ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 136);
            // 
            // 备注NToolStripMenuItem
            // 
            this.备注NToolStripMenuItem.Name = "备注NToolStripMenuItem";
            this.备注NToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.备注NToolStripMenuItem.Text = "重命名(&N)";
            this.备注NToolStripMenuItem.Click += new System.EventHandler(this.备注NToolStripMenuItem_Click);
            // 
            // 预览PToolStripMenuItem
            // 
            this.预览PToolStripMenuItem.Name = "预览PToolStripMenuItem";
            this.预览PToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.预览PToolStripMenuItem.Text = "预览(&P)";
            this.预览PToolStripMenuItem.Click += new System.EventHandler(this.预览PToolStripMenuItem_Click);
            // 
            // 发送至SToolStripMenuItem
            // 
            this.发送至SToolStripMenuItem.Name = "发送至SToolStripMenuItem";
            this.发送至SToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.发送至SToolStripMenuItem.Text = "发送至(&S)";
            // 
            // 设为默认显示ToolStripMenuItem
            // 
            this.设为默认显示ToolStripMenuItem.Name = "设为默认显示ToolStripMenuItem";
            this.设为默认显示ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.设为默认显示ToolStripMenuItem.Text = "设为默认显示(&D)";
            this.设为默认显示ToolStripMenuItem.Click += new System.EventHandler(this.设为默认显示ToolStripMenuItem_Click);
            // 
            // listBoxEstablished
            // 
            this.listBoxEstablished.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxEstablished.ContextMenuStrip = this.contextMenuStrip2;
            this.listBoxEstablished.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxEstablished.FormattingEnabled = true;
            this.listBoxEstablished.HorizontalScrollbar = true;
            this.listBoxEstablished.ItemHeight = 14;
            this.listBoxEstablished.Location = new System.Drawing.Point(325, 49);
            this.listBoxEstablished.Name = "listBoxEstablished";
            this.listBoxEstablished.Size = new System.Drawing.Size(293, 340);
            this.listBoxEstablished.TabIndex = 1;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.断开SToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(116, 26);
            // 
            // 断开SToolStripMenuItem
            // 
            this.断开SToolStripMenuItem.Name = "断开SToolStripMenuItem";
            this.断开SToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.断开SToolStripMenuItem.Text = "断开(&S)";
            this.断开SToolStripMenuItem.Click += new System.EventHandler(this.断开SToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip3;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "远程屏幕监控管理端";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出EToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(116, 26);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 407);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(657, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 17);
            // 
            // 断开重连CToolStripMenuItem
            // 
            this.断开重连CToolStripMenuItem.Name = "断开重连CToolStripMenuItem";
            this.断开重连CToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.断开重连CToolStripMenuItem.Text = "断开重连(&C)";
            this.断开重连CToolStripMenuItem.Click += new System.EventHandler(this.断开重连CToolStripMenuItem_Click);
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 429);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listBoxEstablished);
            this.Controls.Add(this.listBoxUsable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormServer";
            this.Text = "远程屏幕监控管理端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxUsable;
        private System.Windows.Forms.ListBox listBoxEstablished;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 预览PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发送至SToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 断开SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 备注NToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设为默认显示ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripMenuItem 断开重连CToolStripMenuItem;
    }
}

