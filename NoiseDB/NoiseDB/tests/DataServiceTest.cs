using System.Collections.Generic;
using NUnit.Framework;

namespace NoiseDB.Tests
{
    [TestFixture]
    class DataServiceTest
    {

        public DataServiceTest()
        {

        }

        
        [Test]
        public void TestGetValue()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetValue("TestKey").RetrievedData);
        }

        [Test]
        public void TestSetValue()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            Assert.AreEqual("TestValue", dataService.GetValue("TestKey").RetrievedData);
        }

        [Test]
        public void TestDeleteValue()
        {
            DataService dataService = new DataService();
            dataService.SetValue("TestKey", "TestValue");
            dataService.DeleteValue("TestKey");
            Assert.IsInstanceOf(typeof(KeyNotFoundException), dataService.GetValue("TestKey").ThrownException);
        }
        

    }
}
