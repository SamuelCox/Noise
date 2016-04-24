namespace NoiseDB
{
    internal interface IQueryTcpServer
    {
        IQueryService QueryService { get; set; }


        QueryResult StartListener();


        QueryResult StopListener();


        

    }
        
}
