using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    
    public class QueryResult
    {
        public Exception ThrownException { get; set; }
        public string ResultMessage { get; set; }
        public string RetrievedData { get; set; }
        
        public QueryResult(string message,Exception thrownException, string retrievedData)
        {
            ThrownException = thrownException;
            ResultMessage = message;
            RetrievedData = retrievedData;
        }

        public override string ToString()
        {
            if(ThrownException == null)
            {
                return ResultMessage + " " + RetrievedData;
            }
            else
            {
                return ResultMessage + " with Exception : " + ThrownException.ToString();
            }
        }

    }
}
