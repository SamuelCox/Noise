using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class QueryService : IQueryService
    {
        private IDataService dataService;
        
        public QueryService(IDataService dataService)
        {
            this.dataService = dataService;
        }


        public Query ConstructQuery(string queryString)
        {
            string[] query = queryString.Split(',');
            Commands queryCommand = (Commands)Enum.Parse(typeof(Commands), query[0]);
            string queryKey = query[1];
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
                    return dataService.GetRow(query.Key);
                case Commands.SET:
                    return dataService.SetValue(query.Key, query.Argument);                    
                case Commands.DELETE:
                    return dataService.DeleteRow(query.Key);                   
                default:
                    return new QueryResult("Unrecognised command", null, null);                
            }
        }

        
    }
}
