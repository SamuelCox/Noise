using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class DataService : IDataService
    {
        private BinarySerializableDictionary<string, string> KeyValueStore { get; set; }
        private DiskService DiskService { get; set; }
        

        public DataService(DiskService diskService)
        {
            DiskService = diskService;
            KeyValueStore = new BinarySerializableDictionary<string, string>();
        }

        public QueryResult GetRow(string key)
        {

            string result;
            QueryResult queryResult;
            try
            {
                result = KeyValueStore[key];
            }
            catch(Exception e)
            {
                queryResult = new QueryResult("Failed", e, null);
                return queryResult;
            }
            List<string> results = new List<string>();
            results.Add(result);
            queryResult = new QueryResult("Success", null, results);
            return queryResult;
        }

        public QueryResult SetValue(string key, string value)
        {
            KeyValueStore[key] = value;
            return new QueryResult("Success",null,null);
        }

        public QueryResult DeleteRow(string key)
        {
            KeyValueStore[key] = null;
            return new QueryResult("Success", null, null);
        }

       
    }
}
