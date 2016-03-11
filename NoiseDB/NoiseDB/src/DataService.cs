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
        

        public DataService()
        {
            KeyValueStore = new BinarySerializableDictionary<string, string>();
        }

        public QueryResult GetRow(string key)
        {

            string result;
            QueryResult queryResult;
            if(!string.IsNullOrEmpty(key) && KeyValueStore.ContainsKey(key))
            {
                bool success = KeyValueStore.TryGetValue(key, out result);
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
                KeyValueStore.AddOrUpdate(key, value,
                (updateKey, existingVal) =>
                { 
                    return existingVal;
                });
                return new QueryResult("Success", null, null);
            }
            else 
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
        }

        public QueryResult DeleteRow(string key)
        {
            string result;
            if (!string.IsNullOrEmpty(key))
            {
                KeyValueStore.TryRemove(key,out result);
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
