using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.IO;
using System.Configuration;

namespace NoiseDB
{
    internal class QueryTcpServer : IQueryTcpServer
    {
        private bool ServerStarted { get; set; }        
        public IQueryService QueryService { get; set; }
        private bool UseTls { get; set; }
        private readonly int ByteArraySize = int.Parse(ConfigurationManager.AppSettings["ByteArraySize"]);


        public QueryTcpServer()
        {
            UseTls = bool.Parse(ConfigurationManager.AppSettings["UseTls"]);
            
        }
        
        

        public QueryResult StartListener()
        {
            if (!ServerStarted)
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                TcpListener listener = new TcpListener(ipAddress, 4044);
                listener.Start();
                ServerStarted = true;
                new Task(() => ListenForAndProcessConnections(listener)).Start();
                return new QueryResult("Success", null, null);
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }


        }

        public QueryResult StopListener()
        {
            return new QueryResult("Success", null, null);
        }

        private void ListenForAndProcessConnections(TcpListener listener)
        {
            while (true)
            {

                Thread.Sleep(10);
                TcpClient tcpClient = listener.AcceptTcpClient();
                NetworkStream stream = tcpClient.GetStream();
                if (UseTls)
                {
                    string serverCertificateFilePath = ConfigurationManager.AppSettings["ServerTlsCertificateFilePath"];
                    SslStream sslStream = new SslStream(stream);
                    X509Certificate2 certificate = new X509Certificate2(serverCertificateFilePath);
                    sslStream.AuthenticateAsServer(certificate, true, SslProtocols.Tls12, false);
                    new Task(() => ReceiveQueryAndSendResult(sslStream)).Start();
                }
                else
                {
                    new Task(() => ReceiveQueryAndSendResult(stream)).Start();
                }
            }
        }

        private void ReceiveQueryAndSendResult(Stream stream)
        {

            byte[] requestByteBuffer = new byte[ByteArraySize];
            byte[] responseByteBuffer = new byte[ByteArraySize];
            using (stream)
            {
                while (true)
                {                    
                    int requestBytes = stream.Read(requestByteBuffer, 0, requestByteBuffer.Length);
                    string jsonSerializedQuery = Encoding.ASCII.GetString(requestByteBuffer, 0, requestBytes);
                    Query query = null;
                    QueryResult queryResult;
                    string queryResultJson = string.Empty;

                    try
                    {
                        query = JsonConvert.DeserializeObject<Query>(jsonSerializedQuery);
                        queryResult = QueryService.ExecuteQuery(query);
                        queryResultJson = JsonConvert.SerializeObject(queryResult);
                    }

                    catch (JsonException e)
                    {
                        QueryResult errorQueryResult = new QueryResult("Failed", e, null);
                        queryResultJson = JsonConvert.SerializeObject(errorQueryResult);
                    }

                    finally
                    {
                        int responseByteBufferSize = Encoding.ASCII.GetByteCount(queryResultJson);
                        responseByteBuffer = Encoding.ASCII.GetBytes(queryResultJson);
                        stream.Write(responseByteBuffer, 0, responseByteBufferSize);
                        stream.Flush();
                    }

                    if (query?.Command == Commands.SERVER_DISCONNECT)
                    {
                        break;
                    }
                }
            }
        }

        
        



    }
}
