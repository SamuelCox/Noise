using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
            if(!string.IsNullOrEmpty(key) && KeyValueStore.ContainsKey(key))
            {
                result = KeyValueStore[key];
            }
            else 
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
            List<string> results = new List<string>();
            results.Add(result);
            queryResult = new QueryResult("Success", null, results);
            return queryResult;
        }

        public QueryResult SetValue(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                KeyValueStore[key] = value;

                return new QueryResult("Success", null, null);
            }
            else 
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
        }

        public QueryResult DeleteRow(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                KeyValueStore[key] = null;
                return new QueryResult("Success", null, null);
            }
            else 
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
        }

        public QueryResult SaveStore(string name)
        {
            bool success = BinarySerializableDictionary<string, string>.Serialize(KeyValueStore, ConfigurationManager.AppSettings["DataStoreFilePath"] + name);
            if(success)
            {
                return new QueryResult("Success", null, null);
            }
            else 
            {
                return new QueryResult("Failed", null, null);
            }
        }

        public QueryResult LoadStore(string name)
        {
            KeyValueStore = BinarySerializableDictionary<string, string>.Deserialize(ConfigurationManager.AppSettings["DataStoreFilePath"] + name);
            bool success = true;
            if(success)
            {
                return new QueryResult("Success", null, null);
            }
            else
            {
                return new QueryResult("Failed", null, null);
            }
        }

       
    }
}
