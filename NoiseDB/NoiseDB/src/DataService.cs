using System;
using System.Collections.Generic;
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

            string result = string.Empty;            
            bool success = false;

            if (KeyValueStore.ContainsKey(key))
            {
                success = KeyValueStore.TryGetValue(key, out result);
            }
            else
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
            
            if(success)
            {
                return new QueryResult("Success", null, result);
            }            
            else
            {
                return new QueryResult("Failed", null, null);
            }
            
        }

        public QueryResult SetValue(string key, string value)
        {            
            KeyValueStore.AddOrUpdate(key, value,
            (updateKey, existingVal) =>
            { 
                return value;
            });
            return new QueryResult("Success", null, null);                        
        }

        public QueryResult DeleteRow(string key)
        {
            string result;            
            KeyValueStore.TryRemove(key,out result);
            return new QueryResult("Success", null, null);                                        
        }

        public QueryResult SaveStore(string name)
        {
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            try
            {
                BinarySerializableDictionary<string, string>.Serialize(KeyValueStore, filePath);
                return new QueryResult("Success", null, null);
            }
            catch(Exception e)
            {
                return new QueryResult("Failed", e, null);
            }                
            
            
        }

        public QueryResult LoadStore(string name)
        {
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            try
            {
                KeyValueStore = BinarySerializableDictionary<string, string>.Deserialize(filePath);
                return new QueryResult("Success", null, null);
            }
            catch(Exception e)
            {
                return new QueryResult("Failed", e, null);
            }  
            
        }

       
    }
}
