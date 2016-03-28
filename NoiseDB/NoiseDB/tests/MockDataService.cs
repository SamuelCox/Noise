namespace NoiseDB.Tests
{
    public class MockDataService : IDataService
    {
        public MockDataService()
        {

        }

        public QueryResult GetValue(string key)
        {
            return new QueryResult("GetSuccess",null,null);
        }


        public QueryResult SetValue(string key, string value)
        {
            return new QueryResult("SetSuccess", null, null);
        }


        public QueryResult DeleteRow(string key)
        {
            return new QueryResult("DeleteSuccess", null, null);
        }

        public QueryResult SaveStore(string path)
        {
            return new QueryResult("SaveSuccess", null, null);
        }

        public QueryResult LoadStore(string name)
        {
            return new QueryResult("LoadSuccess", null, null);
        }
        

    }
}
