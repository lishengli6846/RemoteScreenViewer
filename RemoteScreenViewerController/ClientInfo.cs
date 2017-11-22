using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteScreenViewerController
{
    public class ClientInfo
    {
        public string IP;
        public System.Net.Sockets.Socket Socket;
        public string Name;
        public string State;

        public ClientInfo(string ip, string name, System.Net.Sockets.Socket socket,string state="")
        {
            this.IP = ip;
            this.Name = name;
            this.Socket = socket;
            this.State = state;
        }

        public override string ToString()
        {
            string re =  string.IsNullOrEmpty(Name) ? IP : Name;
            if(string.IsNullOrEmpty(State)==false && State.Contains("空闲")==false)
                re +="  (" + State + ")";
            return re;
        }
    }
}
