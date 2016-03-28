namespace NoiseDB.Tests
{
    internal class MockQueryTcpServer : IQueryTcpServer
    {
        public IQueryService QueryService {get; set;}

        public MockQueryTcpServer()
        {
            
        }

        public QueryResult StartListener()
        {
            return new QueryResult("ListenSuccess", null, null);
        }

        public QueryResult StopListener()
        {
            return new QueryResult("StopListenSuccess", null, null);
        }

    }
}
