namespace NoiseDB
{
    internal interface IQueryService
    {
        Query ConstructQuery(string query);

        QueryResult ExecuteQuery(Query query);

    }
}
