using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class ServiceManager
    {
        internal IDataService DataService { get; private set;}
        internal IQueryService QueryService { get; private set;}
        
        public QueryTcpServer ConnectionService { get; private set;}

        public ServiceManager()
        {

        }

        public void InitServices()
        {
        
            DataService = new DataService();            
            QueryService = new QueryService();            

        }
    }
}
