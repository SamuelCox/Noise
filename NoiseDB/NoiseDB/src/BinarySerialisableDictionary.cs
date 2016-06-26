using ProtoBuf;
using System.Collections.Concurrent;
using System.IO;

namespace NoiseDB
{
    /// <summary>
    /// A Generic Concurrent Dictionary from the .NET FCL that exposes
    /// static serialize and deserialize methods. Technically speaking 
    /// we don't need this class and could just use the base, but
    /// this feels cleaner.
    /// </summary>
    /// <typeparam name="TKey">The generic type to use for the Dictionary's keys.</typeparam>
    /// <typeparam name="TValue">The generic type to use for the Dictionary's values.</typeparam>
    [ProtoContract]
    internal class BinarySerializableDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {

        //Empty constructor that just calls the base constructor.
        public BinarySerializableDictionary() : base()
        {

        }

        
        /// <summary>
        /// A method that serializes this object to disk at the given filepath.
        /// </summary>
        /// <param name="dictionary">The Dictionary to serialize</param>
        /// <param name="filepath">The filepath to serialize that Dictionary to.</param>        
        public static void Serialize(ConcurrentDictionary<TKey, TValue> dictionary, string filepath)
        {
            using (FileStream stream = new FileStream(filepath, FileMode.Create))
            {
                Serializer.Serialize(stream, dictionary);
            }            
        }

        /// <summary>
        /// A method that deserializes a file on disk into
        /// the Dictionary object it was originally
        /// serialized from, and returns it.
        /// </summary>
        /// <param name="path">The filepath to the Dictionary you want
        /// to deserialize.</param>
        /// <returns>The deserialized dictionary.</returns>
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
