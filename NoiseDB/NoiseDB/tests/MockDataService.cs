using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class MockDataService : IDataService
    {
        public MockDataService()
        {

        }

        public QueryResult GetRow(string key)
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
