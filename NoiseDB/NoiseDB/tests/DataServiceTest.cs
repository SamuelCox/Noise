using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoiseDB
{
    [TestFixture]
    class DataServiceTest
    {

        public DataServiceTest()
        {

        }

        
        [Test]
        public void GetRow()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetRow("TestKey"));
        }

        [Test]
        public void TestSetValue()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetRow("TestKey"));
        }

        [Test]
        public void TestDeleteRow()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            dataService.DeleteRow("TestKey");
            Assert.AreEqual(null, dataService.GetRow("TestKey"));
        }
        

    }
}
