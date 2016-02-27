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
            QueryService queryService = new QueryService(new MockDataService(),null);
            Query query = queryService.ConstructQuery(testQuery);
            string[] queryParts = testQuery.Split(',');
            Assert.AreEqual(query.Command, queryParts[0]);
            Assert.AreEqual(query.Key, queryParts[1]);
            Assert.AreEqual(query.Argument, queryParts[2]);
            
        }

        [TestCaseSource("GetExecuteTestCases")]
        [Test]
        public void TestExecuteQuery(Query query)
        {
            QueryService queryService = new QueryService(new MockDataService(), null);
            QueryResult queryResult = queryService.ExecuteQuery(query);
            if (query.Command == Commands.GET)
            {
                Assert.AreEqual(queryResult.ResultMessage, "GetSuccess");
            }
            else if(query.Command == Commands.SET) 
            {
                Assert.AreEqual(queryResult.ResultMessage, "SetSuccess");
            }
            else
            {
                Assert.AreEqual(queryResult.ResultMessage, "DeleteSuccess");
            }

        }

        public IEnumerable<Query> GetExecuteTestCases
        {
            get
            {
                yield return new Query(Commands.GET, "USERS:1000", string.Empty);
                yield return new Query(Commands.SET, "PRODUCTS:100", "Visual Studio");
                yield return new Query(Commands.DELETE, "PRODUCTS:100", string.Empty);
            }
        }


    }
}
