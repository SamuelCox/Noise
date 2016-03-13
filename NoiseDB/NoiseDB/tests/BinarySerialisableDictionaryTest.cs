using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Configuration;
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
            BinarySerializableDictionary<string, string> dict = new BinarySerializableDictionary<string, string>();
            dict.TryAdd("hi", "yes");
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"];
            BinarySerializableDictionary<string, string>.Serialize(dict, filePath + @"\dict.bin");
            FileAssert.Exists(@"C:\Users\Noiiise\Desktop\desktopshit\dict.bin");
        }
        
        [Test]
        public void TestDeserialize()
        {
            BinarySerializableDictionary<string, string> dict = new BinarySerializableDictionary<string, string>();
            string filePath = ConfigurationManager.AppSettings["DataStoreFilePath"];
            dict = BinarySerializableDictionary<string, string>.Deserialize(filePath + @"\dict.bin");
            Assert.AreEqual("yes", dict["hi"]);
        }
    }
}
