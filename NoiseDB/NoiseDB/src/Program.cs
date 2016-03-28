using System;

namespace NoiseDB
{
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
