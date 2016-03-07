using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class DiskService
    {
        public DiskService()
        {

        }

        public bool OutputDictionaryToDisk(BinarySerializableDictionary<string, string> dictionary, string path)
        {
            try
            {
                BinarySerializableDictionary<string, string>.Serialize(dictionary, path);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public BinarySerializableDictionary<string, string> ReadDictionaryFromFile(string path)
        {
            return BinarySerializableDictionary<string, string>.Deserialize(path);
        }
    }
}
