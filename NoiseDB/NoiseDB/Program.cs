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
           while(true)
           {
               new QueryService(new DataService(new DiskService())).ConstructQuery(Console.ReadLine());
           }
            
        }
    }
}
