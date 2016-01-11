using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    class QueryService
    {

        public QueryService()
        {

        }

        public void ConstructQuery(string queryString)
        {
            string[] query = queryString.Split(' ');
            string command = query[0];
            string argument = query[1];
            string value;
            if(command == Commands.SET.ToString())
            {
                value = query[2];
            }
        }

        public void ExecuteQuery(string queryString)
        {

        }

        public void AddParametersToString(string queryString)
        {

        }
    }
}
