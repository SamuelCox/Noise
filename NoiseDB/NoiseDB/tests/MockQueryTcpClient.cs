namespace NoiseDB.Tests
{
    internal class MockQueryTcpClient : IQueryTcpClient
    {

        public QueryResult Connect(string hostName)
        {
            return new QueryResult("ConnectSuccess", null, null);        
        }

        public QueryResult SendQueryAndReturnResult(Query query)
        {
            return new QueryResult("RemoteQuerySuccess", null, null);
        }
    }
}
