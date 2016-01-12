using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public interface IDataService
    {
        string GetRow(string key);
        

        bool SetValue(string key, object value);
        

        bool DeleteRow(string key);
        

        void GetAllRows();
        

        
    }
}
