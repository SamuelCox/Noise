using NoiseDB.src;
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
            ConnectionService connectionService = new ConnectionService();
            QueryService queryService = new QueryService(new DataService(), connectionService);
            connectionService.QueryService = queryService;
            string input = null;
            while (true)
            {
                input = Console.ReadLine();
                Query query = Parser.ParseQuery(input);
                LoggingService.LogToDisk(query);
                QueryResult result = queryService.ExecuteQuery(query);
                Console.WriteLine(result.ToString());
            }
        }
    }
}
