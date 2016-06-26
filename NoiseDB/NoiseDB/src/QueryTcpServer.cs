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
using System;

namespace NoiseDB
{
    /// <summary>
    /// A class that deals with being the NoiseDB server,
    /// that receives queries over the network, executes them,
    /// then sends the results over the network.
    /// </summary>
    internal class QueryTcpServer : IQueryTcpServer
    {
        //Whether the server is running.
        private bool ServerStarted { get; set; }
        //The QueryService to use to execute the Queries.        
        public IQueryService QueryService { get; set; }
        //Whether to use a connection encrypted over Tls.
        private bool UseTls { get; set; } = bool.Parse(ConfigurationManager.AppSettings["UseTls"]);
        //The size of the byte arrays to use for sending data over the network.
        private readonly int ByteArraySize = int.Parse(ConfigurationManager.AppSettings["ByteArraySize"]);
        //The underlying TcpListener the server uses.
        private TcpListener Listener { get; set; }

        /// <summary>
        /// An empty constructor.
        /// </summary>
        public QueryTcpServer()
        {            
            
        }                

        /// <summary>
        /// A method that starts the TcpListener,
        /// listening on the machine the server is running on, 
        /// on port 4044.
        /// </summary>
        /// <returns>A queryresult on whether the operation succeeded
        /// or not.</returns>
        public QueryResult StartListener()
        {
            if (!ServerStarted)
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                Listener = new TcpListener(ipAddress, 4044);
                Listener.Start();
                ServerStarted = true;
                new Task(() => ListenForAndProcessConnections()).Start();
                return new QueryResult("Success", null, null);
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }


        }

        /// <summary>
        /// A method that stops the TcpListener.
        /// </summary>
        /// <returns></returns>
        public QueryResult StopListener()
        {
            ServerStarted = false;
            Listener.Stop();
            return new QueryResult("Success", null, null);
        }

        /// <summary>
        /// A method that constantly loops,
        /// listens for incoming Tcp connections
        /// over the Listener, accepts them, and
        /// then spins off a new Task to process
        /// that connection.
        /// </summary>
        /// <param name="listener">
        /// The Listener to check for incoming connections over.
        /// </param>
        private void ListenForAndProcessConnections()
        {
            while (true)
            {
                TcpClient tcpClient;
                Thread.Sleep(10);
                try
                {
                    tcpClient = Listener.AcceptTcpClient();
                }
                catch(Exception e)
                {
                    System.Diagnostics.EventLog.WriteEntry("Application", e.ToString());
                    break;
                }
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

        /// <summary>
        /// A method that receives a Query over the network,
        /// passes it to the QueryService to execute it,
        /// and then serialises the result of that execution
        /// and sends it back over the network to the 
        /// client that sent it.
        /// </summary>
        /// <param name="stream">The stream to read queries off of,
        /// and to send queryresults back over.</param>
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
