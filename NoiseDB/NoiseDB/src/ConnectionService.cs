using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

namespace NoiseDB
{
    public class ConnectionService
    {
        private bool ServerStarted { get; set; }
        private TcpListener Listener { get; set;}
        private TcpClient Client { get; set; }
        public QueryService QueryService { private get; set; }

        public ConnectionService()
        {
            
        }
        
        

        public QueryResult ListenForConnection()
        {
            if (!ServerStarted)
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                TcpListener listener = new TcpListener(ipAddress, 4044);
                listener.Start();
                ServerStarted = true;                
                while (true)
                {
                    
                    Thread.Sleep(10);
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ProcessListener, tcpClient);                   
                }
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }


        }

        public void ProcessListener(Object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            byte[] requestByteBuffer = new byte[1024];
            NetworkStream stream = client.GetStream();
            stream.Read(requestByteBuffer, 0, requestByteBuffer.Length);
            string httpMessage = Encoding.ASCII.GetString(requestByteBuffer, 0, requestByteBuffer.Length);
            Query query = JsonConvert.DeserializeObject<Query>(httpMessage);
            QueryResult queryResult = QueryService.ExecuteQuery(query);
            string queryResultJson = JsonConvert.SerializeObject(queryResult);
            byte[] responseByteBuffer = new byte[1024];
            responseByteBuffer = Encoding.ASCII.GetBytes(queryResultJson);
            stream.Write(responseByteBuffer, 0, responseByteBuffer.Length);
            stream.Flush();
        }

        public QueryResult Connect(Query query)
        {
            Client = new TcpClient("127.0.0.1", 4044);
            return new QueryResult("Success", null, null);


        }

        public QueryResult ProcessRemoteQuery(Query query)
        {
            Byte[] byteBuffer = new Byte[1024];
            string jsonSerializedQuery = JsonConvert.SerializeObject(query);
            byteBuffer = Encoding.ASCII.GetBytes(jsonSerializedQuery);
            NetworkStream stream = Client.GetStream();
            stream.Write(byteBuffer, 0, byteBuffer.Length);
            stream.Flush();
            Byte[] responseByteBuffer = new Byte[1024];
            int responseBytes = stream.Read(responseByteBuffer, 0, responseByteBuffer.Length);
            string jsonDeserializedQueryResult = Encoding.ASCII.GetString(responseByteBuffer, 0, responseBytes);
            QueryResult response = JsonConvert.DeserializeObject<QueryResult>(jsonDeserializedQueryResult);
            return response;
        }
        



    }
}
