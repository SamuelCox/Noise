using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Configuration;
using System.IO;
namespace NoiseDB
{
    
    [TestFixture]
    public class BinarySerialisableDictionaryTest
    {

        public BinarySerialisableDictionaryTest()
        {

        }
       
        [Test]
        public void TestSerialize()
        {
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"];
            if (File.Exists(filePath + @"\testserializedict.bin"))
            {
                File.Delete(filePath + @"\tesetserializedict.bin");
            }
            BinarySerializableDictionary<string, string> dict = new BinarySerializableDictionary<string, string>();
            dict.TryAdd("hi", "yes");            
            BinarySerializableDictionary<string, string>.Serialize(dict, filePath + @"\testserializedict.bin");
            if(File.Exists(filePath + @"\testserializedict.bin"))
            {
                Assert.True(true);
            }
            
        }
        
        [Test]
        public void TestDeserialize()
        {
            BinarySerializableDictionary<string, string> dict = new BinarySerializableDictionary<string, string>();
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"];
            dict = BinarySerializableDictionary<string, string>.Deserialize(filePath + @"\testdeserializedict.bin");
            Assert.AreEqual("yes", dict["hi"]);
        }
    }
}
