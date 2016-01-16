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
        public DiskService DiskService { get; private set;}
        public ConnectionService ConnectionService { get; private set;}

        public ServiceManager()
        {

        }

        public void InitServices()
        {
            DiskService = new DiskService();
            DataService = new DataService(DiskService);
            QueryService = new QueryService(DataService);
            ConnectionService = new ConnectionService();

        }
    }
}
