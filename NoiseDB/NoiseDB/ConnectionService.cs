using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace NoiseDB
{
    class ConnectionService
    {

        public ConnectionService()
        {

        }

        public void ConnectToDataStore(string networkPath)
        {
            TcpClient tcpConnection = new TcpClient();
        }

        public void AcceptConnection()
        {
            TcpListener listener = new TcpListener(4044);
        }

    }
}
