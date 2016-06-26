namespace NoiseDB
{
    /// <summary>
    /// An interface that exposes the contract
    /// all implementations of DataServices must conform
    /// to.
    /// </summary>
    internal interface IDataService
    {

        QueryResult GetValue(string key);        

        QueryResult SetValue(string key, string value);
        
        QueryResult DeleteValue(string key);

        QueryResult SaveStore(string name);

        QueryResult LoadStore(string name);
        

        
        

        
    }
}
