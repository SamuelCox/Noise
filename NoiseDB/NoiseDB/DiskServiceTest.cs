using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoiseDB
{
    [TestFixture]
    class DiskServiceTest
    {

        public DiskServiceTest()
        {

        }

        [Test]
        public void TestOutputDictionaryToDisk()
        {
            DiskService diskService = new DiskService();
            BinarySerializableDictionary<string, string> dictionary = new BinarySerializableDictionary<string,string>();
            diskService.OutputDictionaryToDisk(dictionary, @"C:\Users\Noiiise\Desktop\desktopshit\DiskServiceTest.bin");
            FileAssert.Exists(@"C:\Users\Noiiise\Desktop\desktopshit\DiskServiceTest.bin");
        }

        [Test]
        public void TestReadDictionaryFromFile()
        {
            DiskService diskService = new DiskService();
            BinarySerializableDictionary<string, string> dictionaryToWrite = new BinarySerializableDictionary<string, string>();
            diskService.OutputDictionaryToDisk(dictionaryToWrite, @"C:\Users\Noiiise\Desktop\desktopshit\DiskServiceTest.bin");
            BinarySerializableDictionary<string, string> dictionaryToRead = diskService.ReadDictionaryFromFile(@"C:\Users\Noiiise\Desktop\desktopshit\DiskServiceTest.bin");
            Assert.AreEqual(dictionaryToWrite, dictionaryToRead);
        }

    }
}
