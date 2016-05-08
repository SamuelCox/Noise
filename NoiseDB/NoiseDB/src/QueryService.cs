using System;
using System.Linq;
using System.Text;

namespace NoiseDB
{
    public class QueryService : IQueryService
    {
        private IDataService DataService;
        private IQueryTcpServer QueryTcpServer;
        private IQueryTcpClient QueryTcpClient;
        private bool IsConnectedToNetworkDataStore = false;        

        public QueryService()
        {
            DataService = new DataService();
            QueryTcpClient = new QueryTcpClient();
            QueryTcpServer =  new QueryTcpServer();
            QueryTcpServer.QueryService = this;
            
        }       

        internal QueryService(IDataService dataService, IQueryTcpClient queryTcpClient,
                               IQueryTcpServer queryTcpServer)
        {
            DataService = dataService;
            QueryTcpClient = queryTcpClient;
            QueryTcpServer = queryTcpServer;
            QueryTcpServer.QueryService = this;
        }


        public Query ConstructQuery(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return new Query(Commands.UNKNOWN, null, null);
            }
            string[] query = queryString.Split('"');
            Commands queryCommand = Commands.UNKNOWN;
            Enum.TryParse<Commands>(query[0], true, out queryCommand);
            string queryKey = query.Count() > 1 ? query[1] : null;
            string argument = string.Empty;
            if (queryCommand == Commands.SET)
            {
                if (query.Count() > 4)
                {
                    argument = query[3];
                }
            }
            
            return new Query(queryCommand, queryKey, argument);  
        }
        

        public QueryResult ExecuteQuery(Query query)
        {
            
            if (IsConnectedToNetworkDataStore)
            {
                LoggingService.LogToDisk(query);
                if (query.Command == Commands.SERVER_DISCONNECT)
                {
                    IsConnectedToNetworkDataStore = false;
                }
                return QueryTcpClient.SendQueryAndReturnResult(query);
            }
           
            LoggingService.LogToDisk(query,IsConnectedToNetworkDataStore);
            
            switch (query.Command)
            {
                case Commands.GET:
                    return GetValue(query);

                case Commands.SET:
                    return SetValue(query);
                  
                case Commands.DELETE:
                    return DeleteValue(query);

                case Commands.SERVER_START:
                    return StartServer();
                    

                case Commands.SERVER_STOP:
                    return StopServer();
              
                case Commands.SERVER_CONNECT:
                    return ServerConnect(query);
                    
                case Commands.SERVER_DISCONNECT:
                    IsConnectedToNetworkDataStore = false;
                    return new QueryResult("Success", null, null);

                case Commands.SAVE:
                    return SaveDataStore(query);
               
                case Commands.LOAD:
                    return LoadDataStore(query);

                default:
                    return new QueryResult("Unrecognised Command", null, null);                
             }

            
       }
        
        private QueryResult GetValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.GetValue(query.Key);
        }

        private QueryResult SetValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Argument) || string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(), null);
            }
            return DataService.SetValue(query.Key, query.Argument);

        }

        private QueryResult DeleteValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.DeleteRow(query.Key);
        }

        private QueryResult StartServer()
        {
            return QueryTcpServer.StartListener();
        }

        private QueryResult StopServer()
        {
            return QueryTcpServer.StopListener();
        }

        private QueryResult ServerConnect(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            IsConnectedToNetworkDataStore = true;
            return QueryTcpClient.Connect(query.Key).Result;
        }

        private QueryResult ServerDisconnect(Query query)
        {
            IsConnectedToNetworkDataStore = false;
            return new QueryResult("Success", null, null);
        }

        private QueryResult SaveDataStore(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.SaveStore(query.Key);
        }

        private QueryResult LoadDataStore(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.LoadStore(query.Key);
        }

        
    }
}
