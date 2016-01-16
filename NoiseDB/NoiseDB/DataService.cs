using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class DataService : IDataService
    {
        private BinarySerializableDictionary<string, BinarySerializableDictionary<string, string>> KeyValueStore { get; set; }
        private DiskService DiskService { get; set; }
        

        public DataService(DiskService diskService)
        {
            DiskService = diskService;
            KeyValueStore = new BinarySerializableDictionary<string, BinarySerializableDictionary<string, string>>();
        }

        public QueryResult GetRow(string key)
        {
            return null;
        }

        public QueryResult SetValue(string key, object value)
        {
            return null;
        }

        public QueryResult DeleteRow(string key)
        {
            return null;
        }

        public QueryResult GetAllRows(string key)
        {
            BinarySerializableDictionary<string, string> SingleTable = KeyValueStore[key];
            return new QueryResult("Success", null, SingleTable.Values.ToList<string>());
            
        }
    }
}
