using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class QueryService : IQueryService
    {
        private IDataService DataService;
        private QueryServer QueryServer;
        private bool IsConnectedToNetworkDataStore = false;

        public QueryService(IDataService dataService, QueryServer queryServer)
        {
            DataService = dataService;
            QueryServer = queryServer;
        }


        public Query ConstructQuery(string queryString)
        {
            string[] query = queryString.Split(',');
            Commands queryCommand = Commands.UNKNOWN;
            Enum.TryParse<Commands>(query[0], true, out queryCommand);
            string queryKey = query.Count() > 1 ? query[1] : null;
            string argument = string.Empty;
            if(queryCommand == Commands.SET)
            {
                if (query.Count() == 3)
                {
                    argument = query[2];
                }
            }
            
            return new Query(queryCommand, queryKey, argument);  
        }

        public QueryResult ExecuteQuery(Query query)
        {
            switch(query.Command)
            {
                case Commands.GET:
                    if (string.IsNullOrEmpty(query.Key))
                    {
                        return new QueryResult("Failed", new ArgumentNullException(), null);
                    }
                    return DataService.GetRow(query.Key);

                case Commands.SET:
                    if (string.IsNullOrEmpty(query.Argument) || string.IsNullOrEmpty(query.Key))
                    {
                        return new QueryResult("Failed", new ArgumentNullException(), null);
                    }
                    return DataService.SetValue(query.Key, query.Argument);  
                  
                case Commands.DELETE:
                    if (string.IsNullOrEmpty(query.Key))
                    {
                        return new QueryResult("Failed", new ArgumentNullException(), null);
                    }
                    return DataService.DeleteRow(query.Key);

                case Commands.SERVER_START:                    
                    QueryServer.ListenForConnection();
                    return new QueryResult("blah", null, null);
                    

                case Commands.SERVER_STOP:
                    return new QueryResult("NotImplemented", null, null);      
              
                case Commands.SERVER_CONNECT:
                    IsConnectedToNetworkDataStore = true;
                    return QueryServer.Connect();
                    

                case Commands.SERVER_DISCONNECT:
                    IsConnectedToNetworkDataStore = false;
                    return new QueryResult("NotImplemented", null, null);

                case Commands.SAVE:
                    if (string.IsNullOrEmpty(query.Key))
                    {
                        return new QueryResult("Failed", new ArgumentNullException(), null);
                    }                    
                    return DataService.SaveStore(query.Key);     
               
                case Commands.LOAD:
                    if (string.IsNullOrEmpty(query.Key))
                    {
                        return new QueryResult("Failed", new ArgumentNullException(), null);
                    }                    
                    return DataService.LoadStore(query.Key);

                default:
                    return new QueryResult("Unrecognised command", null, null);                
            }
        }

        
    }
}
