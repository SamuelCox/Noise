using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ProtoBuf;

namespace NoiseDB
{
    [ProtoContract]
    public class BinarySerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public BinarySerializableDictionary()
        {

        }

        protected BinarySerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }

        public static bool Serialize(Dictionary<TKey, TValue> dictionary, string filepath)
        {
            using (FileStream stream = new FileStream(filepath, FileMode.Create))
            {
                Serializer.Serialize(stream, dictionary);
            }
            return true;
        }

        public static BinarySerializableDictionary<TKey, TValue> Deserialize(string path)
        {
            BinarySerializableDictionary<TKey, TValue> dict = new BinarySerializableDictionary<TKey,TValue>();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                dict = Serializer.Deserialize<BinarySerializableDictionary<TKey, TValue>>(stream);
            }
            return dict;
        }
    }
}
