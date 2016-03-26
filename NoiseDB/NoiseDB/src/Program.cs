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
            QueryService queryService = new QueryService(new DataService());            
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
