using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public enum Commands
    {
        UNKNOWN = 0,
        GET = 1,
        SET = 2,
        DELETE = 3,
        SERVER_START = 4,
        SERVER_STOP = 5,
        
    }
}
