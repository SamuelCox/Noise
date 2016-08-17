using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NoiseDB
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryService : IQueryService
    {
        private IDataService DataService;
        private IQueryTcpServer QueryTcpServer;
        private IQueryTcpClient QueryTcpClient;
        private bool IsConnectedToNetworkDataStore = false;
        private readonly bool LoggingEnabled = bool.Parse(ConfigurationManager.AppSettings["LoggingEnabled"]);

        /// <summary>
        /// 
        /// </summary>
        public QueryService()
        {
            DataService = new DataService();
            QueryTcpClient = new QueryTcpClient();
            QueryTcpServer =  new QueryTcpServer();
            QueryTcpServer.QueryService = this;
            
        }       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="queryTcpClient"></param>
        /// <param name="queryTcpServer"></param>
        internal QueryService(IDataService dataService, IQueryTcpClient queryTcpClient,
                               IQueryTcpServer queryTcpServer)
        {
            DataService = dataService;
            QueryTcpClient = queryTcpClient;
            QueryTcpServer = queryTcpServer;
            QueryTcpServer.QueryService = this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
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
                if (query.Count() >= 4)
                {
                    argument = query[3];
                }
            }
            
            return new Query(queryCommand, queryKey, argument);  
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public QueryResult ExecuteQuery(Query query)
        {
            
            if (IsConnectedToNetworkDataStore)
            {
                if (LoggingEnabled)
                {
                    LoggingService.LogToDisk(query, true);
                }
                if (query.Command == Commands.SERVER_DISCONNECT)
                {
                    IsConnectedToNetworkDataStore = false;
                }
                return QueryTcpClient.SendQueryAndReturnResult(query);
            }

            if (LoggingEnabled)
            {
                LoggingService.LogToDisk(query, false);
            }
            
            switch (query.Command)
            {
                case Commands.GET:
                    return GetValue(query);

                case Commands.SET:
                    return SetValue(query);
                  
                case Commands.DELETE:
                    return DeleteValue(query);

                case Commands.SERVER_START:
                    return StartServer(query);
                    

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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult GetValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.GetValue(query.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult SetValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Argument) || string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(), null);
            }
            return DataService.SetValue(query.Key, query.Argument);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult DeleteValue(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.DeleteValue(query.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private QueryResult StartServer(Query query)
        {
            return QueryTcpServer.StartListener(query.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private QueryResult StopServer()
        {
            return QueryTcpServer.StopListener();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult ServerConnect(Query query)
        {
            try
            {
                if (string.IsNullOrEmpty(query.Key))
                {
                    return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
                }


                IsConnectedToNetworkDataStore = true;
                return QueryTcpClient.Connect(query.Key).Result;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult ServerDisconnect(Query query)
        {
            IsConnectedToNetworkDataStore = false;
            return new QueryResult("Success", null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryResult SaveDataStore(Query query)
        {
            if (string.IsNullOrEmpty(query.Key))
            {
                return new QueryResult("Failed", new ArgumentNullException(nameof(query.Key)), null);
            }
            return DataService.SaveStore(query.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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
