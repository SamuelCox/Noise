using Newtonsoft.Json;
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
    /// <summary>
    /// A class that deals with connecting
    /// to a NoiseDB server, and sending queries 
    /// to that server.
    /// </summary>
    internal class QueryTcpClient : IQueryTcpClient
    {
        //The underlying TcpClient to use.
        private TcpClient Client { get; set; }
        //Whether or not to use Tls encrypted connections.        
        private bool UseTls { get; set; } = bool.Parse(ConfigurationManager.AppSettings["UseTls"]);
        //The hostname of the server to connect to.
        private string ConnectedHostName { get; set; }
        //The underlying networkstream to use for the server.
        private Stream ClientStream { get; set; }
        //The size of the ByteArrays to use to transfer data over the network. Will heavily affect performance.
        private readonly int ByteArraySize = int.Parse(ConfigurationManager.AppSettings["ByteArraySize"]);

        /// <summary>
        /// An empty constructor.
        /// </summary>
        public QueryTcpClient()
        {
                        
        }

        /// <summary>
        /// A method that takes a hostname,
        /// and connects to a NoiseDB server
        /// running on that hostname.
        /// </summary>
        /// <param name="hostName">The hostname to connect to.</param>
        /// <returns></returns>
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

        /// <summary>
        /// A method that takes a Query, sends that Query
        /// to the NoiseDB server you're connected to,
        /// waits to receive the QueryResult of executing
        /// that Query, and returns it.
        /// </summary>
        /// <param name="query">The Query to execute on the server.</param>
        /// <returns>
        /// The QueryResult of the executed Query.
        /// </returns>
        public QueryResult SendQueryAndReturnResult(Query query)
        {
            Byte[] byteBuffer = new Byte[ByteArraySize];
            string jsonSerializedQuery = JsonConvert.SerializeObject(query);
            byteBuffer = Encoding.ASCII.GetBytes(jsonSerializedQuery);

            int responseBytes;
            Byte[] responseByteBuffer;
            responseByteBuffer = WriteQueryToStreamAndReadResult(ClientStream, byteBuffer, out responseBytes);
            string jsonSerializedQueryResult = Encoding.ASCII.GetString(responseByteBuffer, 0, responseBytes);
            QueryResult response;

            try
            {
                response = JsonConvert.DeserializeObject<QueryResult>(jsonSerializedQueryResult);
            }
            catch(JsonException e)
            {
                return new QueryResult("Failed", e, null);
            }
            return response;
        }

        /// <summary>
        /// A method that writes a Query to a networkstream,
        /// and sends it over the network. It then reads the
        /// response from that stream, and returns it.
        /// </summary>
        /// <param name="stream">The Stream the network
        /// communication is happening over.</param>
        /// <param name="queryByteArray">The byte array the serialised Query is stored in.</param>
        /// <param name="responseBytes">The number of Bytes to read from the Stream.</param>
        /// <returns></returns>
        private byte[] WriteQueryToStreamAndReadResult(Stream stream, Byte[] queryByteArray, out int responseBytes)
        {
            stream.Write(queryByteArray, 0, queryByteArray.Length);
            stream.Flush();
            byte[] responseByteBuffer = new Byte[ByteArraySize];
            responseBytes = stream.Read(responseByteBuffer, 0, responseByteBuffer.Length);
            return responseByteBuffer;
        }
    }
}
