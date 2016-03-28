namespace NoiseDB
{
    public class ServiceManager
    {
        internal IDataService DataService { get; private set;}
        internal IQueryService QueryService { get; private set;}
        
        internal QueryTcpServer ConnectionService { get; private set;}

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
