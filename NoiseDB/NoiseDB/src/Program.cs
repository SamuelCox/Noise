using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionService queryServer = new ConnectionService();
            QueryService queryService = new QueryService(new DataService(), new ConnectionService());
            queryServer.QueryService = queryService;
            string input = null;
            while (true)
            {
                input = Console.ReadLine();
                Query query = Parser.ParseQuery(input);

                Console.WriteLine(query.ToString());
                QueryResult result = queryService.ExecuteQuery(query);
                Console.WriteLine(result.ToString());
            }
        }
    }
}
