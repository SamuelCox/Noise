using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace NoiseDB
{
    public class ConnectionService
    {
        private bool ServerStarted { get; set; }
        private TcpListener Listener { get; set;}

        public ConnectionService()
        {

        }

        public void ConnectToDataStore(string networkPath)
        {
            TcpClient tcpConnection = new TcpClient();
        }

        public QueryResult ListenForConnection()
        {
            if (!ServerStarted)
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                Listener = new TcpListener(ipAddress, 4044);
                Listener.Start();
                ServerStarted = true;
                Task<bool> task =  new Task<bool>(StartListening);
                task.Start();
                return new QueryResult("Success", null, null);
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }
        }

        public bool StartListening()
        {
            return true;
        }



    }
}
