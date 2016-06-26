using System;
using System.Collections.Generic;
using System.Configuration;

namespace NoiseDB
{
    /// <summary>
    /// An implementation of the IDataService interface,
    /// deals with all the underlying data operations, and is called into by 
    /// QueryService or the QueryTcp classes.
    /// </summary>
    internal class DataService : IDataService
    {
        //A ConcurrentDictionary that provides the storage of data.
        private BinarySerializableDictionary<string, string> KeyValueStore { get; set; }

        /// <summary>
        /// A constructor that initialises the KeyValueStore member variable with
        ///a default capacity.
        /// </summary>
        public DataService()
        {
            KeyValueStore = new BinarySerializableDictionary<string, string>();
        }

        /// <summary>
        /// A method that takes a key, and
        /// tries to get the corresponding value
        /// from the KeyValueStore ConcurrentDictionary.
        /// </summary>
        /// <param name="key">The key to get the value from.</param>
        /// <returns>
        /// Returns a QueryResult containing the retrieved data
        /// (if any) as well as whether the operation succeeded.
        /// </returns>
        public QueryResult GetValue(string key)
        {

            string result = string.Empty;            
            bool success = false;

            if (KeyValueStore.ContainsKey(key))
            {
                success = KeyValueStore.TryGetValue(key, out result);
            }
            else
            {
                return new QueryResult("Failed", new KeyNotFoundException(), null);
            }
            
            if(success)
            {
                return new QueryResult("Success", null, result);
            }            
            else
            {
                return new QueryResult("Failed", null, null);
            }
            
        }

        /// <summary>
        /// A method that takes a key and a value,
        /// and sets the key in the KeyValueStore
        /// to that value.
        /// </summary>
        /// <param name="key">The key the value corresponds to.</param>
        /// <param name="value">The value stored against the key.</param>
        /// <returns>
        /// Returns a QueryResult on whether the operation succeeded
        /// </returns>
        public QueryResult SetValue(string key, string value)
        {            
            KeyValueStore.AddOrUpdate(key, value,
            (updateKey, existingVal) =>
            { 
                return value;
            });
            return new QueryResult("Success", null, null);                        
        }

        /// <summary>
        /// A method that takes a key and deletes (nulls)
        /// any value stored against that key.
        /// </summary>
        /// <param name="key">The key the value is stored against.</param>
        /// <returns>
        /// A queryresult on whether the operation succeeded or not.
        /// </returns>
        public QueryResult DeleteValue(string key)
        {
            string result;            
            KeyValueStore.TryRemove(key,out result);
            return new QueryResult("Success", null, null);                                        
        }

        /// <summary>
        /// A method that takes a string,
        /// and serialises the KeyValueStore
        /// dictionary to the disk saved as
        /// the string passed in.
        /// </summary>
        /// <param name="name">
        /// The name to save the dictionary as.
        /// </param>
        /// <returns>
        /// A queryresult on whether the operation succeeded or not.
        /// </returns>
        public QueryResult SaveStore(string name)
        {
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            try
            {
                BinarySerializableDictionary<string, string>.Serialize(KeyValueStore, filePath);
                return new QueryResult("Success", null, null);
            }
            catch (Exception e)
            {
                return new QueryResult("Failed", e, null);
            }                
            
            
        }

        /// <summary>
        /// A method that takes the name of a file on disk,
        /// and deserialises that into the KeyValueStore
        /// member variable.
        /// </summary>
        /// <param name="name">The name of the dictionary.</param>
        /// <returns>
        /// A queryresult on whether the operation succeeded or not.
        /// </returns>
        public QueryResult LoadStore(string name)
        {
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"] + name;
            try
            {
                KeyValueStore = BinarySerializableDictionary<string, string>.Deserialize(filePath);
                return new QueryResult("Success", null, null);
            }
            catch (Exception e)
            {
                return new QueryResult("Failed", e, null);
            }  
            
        }

       
    }
}
