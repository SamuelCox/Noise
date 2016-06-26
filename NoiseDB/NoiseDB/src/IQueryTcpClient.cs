using System.Threading.Tasks;

namespace NoiseDB
{
    /// <summary>
    /// An interface that exposes the contract
    /// all implementations of QueryTcpClients must conform
    /// to.
    /// </summary>
    internal interface IQueryTcpClient
    {

        Task<QueryResult> Connect(string hostName);

        QueryResult SendQueryAndReturnResult(Query query);


        
        
    }
}
