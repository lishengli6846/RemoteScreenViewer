using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteScreenViewer
{
    /// <summary>
    /// 保存鼠标的坐标
    /// </summary>
    public struct MousePositions
    {
        public int x;
        public int y;
        public int mouseX
        {
            get { return x; }
        }
        public int mouseY
        {
            get { return y; }
        }
    }
}
