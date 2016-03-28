namespace NoiseDB
{
    internal interface IQueryTcpClient
    {

        QueryResult Connect(string hostName);




        QueryResult SendQueryAndReturnResult(Query query);


        
        
    }
}
