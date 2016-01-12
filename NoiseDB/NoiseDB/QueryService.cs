using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class QueryService
    {
        private IDataService dataService;
        public Commands CurrentCommand { get; private set; }
        public string CurrentArgument { get; private set; }
        public string CurrentValue { get; private set; }

        public QueryService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public void ConstructQuery(string queryString)
        {
            string[] query = queryString.Split(',');
            CurrentCommand = (Commands)Enum.Parse(typeof(Commands), query[0]);
            CurrentArgument = query[1];
            if(query[2] == Commands.SET.ToString())
            {
                CurrentValue = query[2];
            }
        }

        public string ExecuteQuery()
        {
            switch(CurrentCommand)
            {
                case Commands.GET:
                    return dataService.GetRow(CurrentArgument);
                case Commands.SET:
                    return dataService.SetValue(CurrentArgument, CurrentValue).ToString();                    
                case Commands.DELETE:
                    return dataService.DeleteRow(CurrentArgument).ToString();                   
                default:
                    return "Unrecognised command";                
            }
        }

        
    }
}
