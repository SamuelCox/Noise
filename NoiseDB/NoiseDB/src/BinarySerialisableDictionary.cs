using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    [ProtoContract]
    public class BinarySerializableDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {

        public BinarySerializableDictionary()
        {

        }

        

        public static bool Serialize(ConcurrentDictionary<TKey, TValue> dictionary, string filepath)
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
