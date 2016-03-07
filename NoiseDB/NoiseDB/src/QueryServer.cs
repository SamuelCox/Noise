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
    public class QueryServer
    {
        private bool ServerStarted { get; set; }
        private TcpListener Listener { get; set;}
        public QueryService QueryService { private get; set; }

        public QueryServer()
        {
            QueryService = new QueryService(new DataService(), this);
        }
        
        

        public QueryResult ListenForConnection()
        {
            if (!ServerStarted)
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                Listener = new TcpListener(ipAddress, 4044);
                Listener.Start();
                ServerStarted = true;                
                while (true)
                {
                    Thread.Sleep(10);
                    TcpClient tcpClient = Listener.AcceptTcpClient();
                    byte[] requestByteBuffer = new byte[256];
                    NetworkStream stream = tcpClient.GetStream();
                    stream.Read(requestByteBuffer, 0, requestByteBuffer.Length);
                    string httpMessage = Encoding.ASCII.GetString(requestByteBuffer, 0, requestByteBuffer.Length);
                    Query query = JsonConvert.DeserializeObject<Query>(httpMessage);
                    QueryResult queryResult = QueryService.ExecuteQuery(query);
                    string queryResultJson = JsonConvert.SerializeObject(queryResult);
                    byte[] responseByteBuffer = new byte[256];
                    responseByteBuffer = Encoding.ASCII.GetBytes(queryResultJson);
                    stream.Write(responseByteBuffer, 0, responseByteBuffer.Length);
                    stream.Flush();

                }
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }


        }

        public QueryResult Connect()
        {
            TcpClient client = new TcpClient("127.0.0.1", 4044);
            Byte[] byteBuffer = new Byte[256];
            string blah = JsonConvert.SerializeObject(QueryService.ConstructQuery("set,users:100,3"));
            byteBuffer = Encoding.ASCII.GetBytes(blah);
            NetworkStream stream = client.GetStream();
            stream.Write(byteBuffer, 0, byteBuffer.Length);
            stream.Flush();
            Byte[] responseByteBuffer = new Byte[256];
            int responseBytes = stream.Read(responseByteBuffer, 0, responseByteBuffer.Length);
            string json = Encoding.ASCII.GetString(responseByteBuffer, 0, responseBytes);
            QueryResult response = JsonConvert.DeserializeObject<QueryResult>(json);
            return response;

        }
        



    }
}
