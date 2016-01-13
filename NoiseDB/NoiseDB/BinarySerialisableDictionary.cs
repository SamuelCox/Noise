using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NoiseDB
{
    [Serializable]
    public class BinarySerializableDictionary<TKey, TValue> : Dictionary<TKey, string>
    {

        public BinarySerializableDictionary()
        {

        }

        protected BinarySerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }

        public static bool Serialize(Dictionary<TKey, TValue> dictionary, string filepath)
        {
            BinaryFormatter bfw = new BinaryFormatter();
            using (FileStream stream = new FileStream(filepath, FileMode.Create))
            {
                bfw.Serialize(stream, dictionary);
            }
            return true;
        }

        public static BinarySerializableDictionary<TKey, TValue> Deserialize()
        {
            BinaryFormatter bfw = new BinaryFormatter();
            BinarySerializableDictionary<TKey, TValue> dict = new BinarySerializableDictionary<TKey,TValue>();
            using (FileStream stream = new FileStream(@"C:\Users\Noiiise\Desktop\desktopshit\dict.bin", FileMode.Open))
            {
                dict = (BinarySerializableDictionary<TKey, TValue>) bfw.Deserialize(stream);
            }
            return dict;
        }
    }
}
