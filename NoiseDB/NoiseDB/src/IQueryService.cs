using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    internal interface IQueryService
    {
        Query ConstructQuery(string query);

        QueryResult ExecuteQuery(Query query);

    }
}
