﻿using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    internal class QueryTcpClient : IQueryTcpClient
    {
        private TcpClient Client { get; set; }        
        private bool UseTls { get; set; }
        private string ConnectedHostName { get; set; }
        private Stream ClientStream { get; set; }

        public QueryTcpClient()
        {
            UseTls = bool.Parse(ConfigurationManager.AppSettings["UseTls"]);
        }

        public async Task<QueryResult> Connect(string hostName)
        {
            ConnectedHostName = hostName;
            Client = new TcpClient(ConnectedHostName, 4044);
            if (UseTls)
            {
                string clientTlsCertificatePath = ConfigurationManager.AppSettings["ClientTlsCertificateFilePath"];
                X509Certificate2 clientCertificate = new X509Certificate2(clientTlsCertificatePath);
                X509CertificateCollection clientCertificateCollection =
                    new X509CertificateCollection(new X509Certificate[] { clientCertificate });
                ClientStream = new SslStream(Client.GetStream());
                await ((SslStream)ClientStream).AuthenticateAsClientAsync(ConnectedHostName, clientCertificateCollection, SslProtocols.Tls12,true);
            }
            else
            {
                ClientStream = Client.GetStream();
            }
            return new QueryResult("Success", null, null);
        }


        public QueryResult SendQueryAndReturnResult(Query query)
        {
            Byte[] byteBuffer = new Byte[1024];
            string jsonSerializedQuery = JsonConvert.SerializeObject(query);
            byteBuffer = Encoding.ASCII.GetBytes(jsonSerializedQuery);
            int responseBytes;
            Byte[] responseByteBuffer;
            responseByteBuffer = WriteQueryToStreamAndReadResult(ClientStream, byteBuffer, out responseBytes);
            string jsonDeserializedQueryResult = Encoding.ASCII.GetString(responseByteBuffer, 0, responseBytes);
            QueryResult response = JsonConvert.DeserializeObject<QueryResult>(jsonDeserializedQueryResult);
            return response;
        }

        private byte[] WriteQueryToStreamAndReadResult(Stream stream, Byte[] queryByteArray, out int responseBytes)
        {
            stream.Write(queryByteArray, 0, queryByteArray.Length);
            stream.Flush();
            byte[] responseByteBuffer = new Byte[1024];
            responseBytes = stream.Read(responseByteBuffer, 0, responseByteBuffer.Length);
            return responseByteBuffer;
        }
    }
}
