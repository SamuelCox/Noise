using System;

namespace NoiseDB
{
    /// <summary>
    /// The Entry-point of both the server
    /// and client NoiseDB application.
    /// This instantiates all services the
    /// app needs, and takes input from the command
    /// line and executes that input.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {            
            QueryService queryService = new QueryService();            
            string input = null;
            while (true)
            {
                input = Console.ReadLine();
                Query query = queryService.ConstructQuery(input);
                QueryResult result = queryService.ExecuteQuery(query);
                Console.WriteLine(result.ToString());
            }
        }
    }
}
