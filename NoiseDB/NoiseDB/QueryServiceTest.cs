using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
namespace NoiseDB
{
    [TestFixture]
    class QueryServiceTest
    {
        public QueryServiceTest()
        {

        }

        [TestCase("GET,USERS:1000")]
        [TestCase("SET,PRODUCTS:100:NAME,Visual Studio ")]
        [TestCase("DELETE,USERS:1000")]
        [Test]
        public void TestConstructQuery(string testQuery)
        {
            QueryService queryService = new QueryService(null);
            queryService.ConstructQuery(testQuery);
            string[] queryParts = testQuery.Split(',');
            Assert.AreEqual(queryService.CurrentCommand, queryParts[0]);
            Assert.AreEqual(queryService.CurrentArgument, queryParts[1]);
            Assert.AreEqual(queryService.CurrentValue, queryParts[2]);
            
        }


    }
}
