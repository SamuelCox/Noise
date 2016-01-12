using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
            dict.Add("hi", "yes");
            BinarySerializableDictionary<string, string>.Serialize(dict);
            FileAssert.Exists(@"C:\Users\Noiiise\Desktop\desktopshit\dict.bin");
        }

        [Test]
        public void TestDeserialize()
        {
            BinarySerializableDictionary<string, string> dict = new BinarySerializableDictionary<string, string>();
            dict = BinarySerializableDictionary<string, string>.Deserialize();
            Assert.AreEqual("yes", dict["hi"]);
        }
    }
}
