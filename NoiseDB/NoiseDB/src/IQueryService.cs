namespace NoiseDB
{
    /// <summary>
    /// An interface that exposes the contract
    /// all implementations of QueryServices must conform
    /// to.
    /// </summary>
    internal interface IQueryService
    {
        Query ConstructQuery(string query);

        QueryResult ExecuteQuery(Query query);

    }
}
