using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteScreenViewerController
{
    class ScreenViewInfo
    {
        public ClientInfo srcClient;
        public ClientInfo destClient;

        public ScreenViewInfo(ClientInfo src, ClientInfo dest)
        {
            srcClient = src;
            destClient = dest;
        }

        public override string ToString()
        {
            return srcClient.ToString() + "  -->  " + destClient.ToString();
        }
    }
}
