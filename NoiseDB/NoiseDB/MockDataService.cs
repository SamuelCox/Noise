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

        public string GetRow(string key)
        {
            return string.Empty;
        }


        public bool SetValue(string key, object value)
        {
            return true;
        }


        public bool DeleteRow(string key)
        {
            return true;
        }


        public void GetAllRows()
        {

        }
        

    }
}
