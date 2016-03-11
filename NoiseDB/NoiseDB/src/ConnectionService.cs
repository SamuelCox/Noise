﻿using System;
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
                    new Task(() => ProcessListener(tcpClient)).Start();                    
                }
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }


        }

        public void ProcessListener(TcpClient tcpClient)
        {

            byte[] requestByteBuffer = new byte[1024];
            byte[] responseByteBuffer = new byte[1024];
            while (true)
            {
                Thread.Sleep(10);
                NetworkStream stream = tcpClient.GetStream();
                int requestBytes = stream.Read(requestByteBuffer, 0, requestByteBuffer.Length);
                string httpMessage = Encoding.ASCII.GetString(requestByteBuffer, 0, requestBytes);
                Query query = JsonConvert.DeserializeObject<Query>(httpMessage);
                if(query.Command == Commands.SERVER_DISCONNECT)
                {
                    break;
                }
                QueryResult queryResult = QueryService.ExecuteQuery(query);
                string queryResultJson = JsonConvert.SerializeObject(queryResult);                
                responseByteBuffer = Encoding.ASCII.GetBytes(queryResultJson);
                stream.Write(responseByteBuffer, 0, responseByteBuffer.Length);
                stream.Flush();
            }
        }

        public QueryResult Connect()
        {
            Client = new TcpClient("127.0.0.1", 4044);
            return new QueryResult("Success", null, null);
        }

        public void Disconnect()
        {
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
