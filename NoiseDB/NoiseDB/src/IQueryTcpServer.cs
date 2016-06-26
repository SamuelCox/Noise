namespace NoiseDB
{
    /// <summary>
    /// An interface that exposes the contract
    /// all implementations of QueryTcpServers must conform
    /// to.
    /// </summary>
    internal interface IQueryTcpServer
    {
        IQueryService QueryService { get; set; }

        QueryResult StartListener();

        QueryResult StopListener();


        

    }
        
}
