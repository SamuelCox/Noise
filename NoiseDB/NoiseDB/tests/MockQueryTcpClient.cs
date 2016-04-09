using System.Threading.Tasks;

namespace NoiseDB.Tests
{
    internal class MockQueryTcpClient : IQueryTcpClient
    {

        public async Task<QueryResult> Connect(string hostName)
        {
            
            return new QueryResult("ConnectSuccess", null, null);        
        }

        public QueryResult SendQueryAndReturnResult(Query query)
        {
            return new QueryResult("RemoteQuerySuccess", null, null);
        }
    }
}
