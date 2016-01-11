using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    class BinarySerialisableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public static bool Serialise()
        {
            return true;
        }

        public static BinarySerialisableDictionary<TKey, TValue> Deserialise()
        {
            return null;
        }
    }
}
