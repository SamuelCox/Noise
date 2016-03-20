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
        public void TestGetRow()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetRow("TestKey").RetrievedData);
        }

        [Test]
        public void TestSetValue()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetRow("TestKey").RetrievedData);
        }

        [Test]
        public void TestDeleteRow()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            dataService.DeleteRow("TestKey");
            Assert.IsInstanceOf(typeof(KeyNotFoundException), dataService.GetRow("TestKey").ThrownException);
        }
        

    }
}
