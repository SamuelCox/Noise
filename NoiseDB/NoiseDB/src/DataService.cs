using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NoiseDB
{
    internal class DataService : IDataService
    {
        private BinarySerializableDictionary<string, string> KeyValueStore { get; set; }
        

        public DataService()
        {
            KeyValueStore = new BinarySerializableDictionary<string, string>();
        }

        public QueryResult GetValue(string key)
        {

            string result;
            QueryResult queryResult;
            if(!string.IsNullOrEmpty(key) && KeyValueStore.ContainsKey(key))
            {
               KeyValueStore.TryGetValue(key, out result);
            }
            else 
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }            
            queryResult = new QueryResult("Success", null, result);
            return queryResult;
        }

        public QueryResult SetValue(string key, string value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                KeyValueStore.AddOrUpdate(key, value,
                (updateKey, existingVal) =>
                { 
                    return value;
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
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            bool success = BinarySerializableDictionary<string, string>.Serialize(KeyValueStore, filePath);
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
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            KeyValueStore = BinarySerializableDictionary<string, string>.Deserialize(filePath);
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
