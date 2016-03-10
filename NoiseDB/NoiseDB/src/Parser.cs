using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class Parser
    {
        public static Query ParseQuery(string queryString)
        {
            string[] query = queryString.Split(',');
            Commands queryCommand = Commands.UNKNOWN;
            Enum.TryParse<Commands>(query[0], true, out queryCommand);
            string queryKey = query.Count() > 1 ? query[1] : null;
            string argument = string.Empty;
            if (queryCommand == Commands.SET)
            {
                if (query.Count() == 3)
                {
                    argument = query[2];
                }
            }

            return new Query(queryCommand, queryKey, argument);  
        }

        public static bool IsNetworkQuery(Query query)
        {
            Commands queryCommand = query.Command;
            if(queryCommand == Commands.SERVER_CONNECT || queryCommand == Commands.SERVER_DISCONNECT || queryCommand == Commands.SERVER_START || queryCommand == Commands.SERVER_STOP)
            {
                return true;
            }
            return false;
        }
        public Parser()
        {

        }
    }
}
