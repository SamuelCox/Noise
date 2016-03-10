using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class ServiceManager
    {
        public IDataService DataService { get; private set;}
        public IQueryService QueryService { get; private set;}
        
        public ConnectionService ConnectionService { get; private set;}

        public ServiceManager()
        {

        }

        public void InitServices()
        {
        
            DataService = new DataService();
            ConnectionService = new ConnectionService();
            QueryService = new QueryService(DataService, ConnectionService);            

        }
    }
}
