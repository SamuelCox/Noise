namespace NoiseDB
{
    internal interface IDataService
    {

        QueryResult GetValue(string key);
        

        QueryResult SetValue(string key, string value);
        

        QueryResult DeleteRow(string key);

        QueryResult SaveStore(string name);

        QueryResult LoadStore(string name);
        

        
        

        
    }
}
