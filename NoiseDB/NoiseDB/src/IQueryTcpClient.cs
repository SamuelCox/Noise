using System.Threading.Tasks;

namespace NoiseDB
{
    internal interface IQueryTcpClient
    {

        Task<QueryResult> Connect(string hostName);

        QueryResult SendQueryAndReturnResult(Query query);


        
        
    }
}
