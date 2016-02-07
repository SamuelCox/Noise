using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public interface IDataService
    {

        QueryResult GetRow(string key);
        

        QueryResult SetValue(string key, string value);
        

        QueryResult DeleteRow(string key);
        

        
        

        
    }
}
