using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class DataService : IDataService
    {
        private BinarySerializableDictionary<string, BinarySerializableDictionary<string, string>> KeyValueStore { get; set;}

        public DataService()
        {
           
        }

        public string GetRow(string key)
        {
            return null;
        }

        public bool SetValue(string key, object value)
        {
            return false;
        }

        public bool DeleteRow(string key)
        {
            return false;
        }

        public void GetAllRows()
        {

        }
    }
}
