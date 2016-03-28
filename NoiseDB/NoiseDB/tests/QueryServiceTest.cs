using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NoiseDB.Tests
{
    [TestFixture]
    public class QueryServiceTest
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
            QueryService queryService = new QueryService();
            Query query = queryService.ConstructQuery(testQuery);
            string[] queryParts = testQuery.Split(',');
            Assert.AreEqual(query.Command.ToString(), queryParts[0]);
            if (queryParts.Count() > 1)
            {
                Assert.AreEqual(query.Key, queryParts[1]);
            }
            if(queryParts.Count() > 2)
            {
                Assert.AreEqual(query.Argument, queryParts[2]);
            }
            
            
        }

        
        [Test]
        public void TestExecuteQuery()
        {
            QueryService queryService = new QueryService(new MockDataService(), new MockQueryTcpClient(),
                                                         new MockQueryTcpServer());
            List<KeyValuePair<Query, QueryResult>> queryTestData = GetExecute.ToList<KeyValuePair<Query, QueryResult>>();
            foreach (KeyValuePair<Query, QueryResult> keyValuePair in queryTestData)
            {
                QueryResult queryResult = queryService.ExecuteQuery(keyValuePair.Key);
                Assert.AreEqual(keyValuePair.Value.ResultMessage, queryResult.ResultMessage);
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


        public IEnumerable<KeyValuePair<Query, QueryResult>> GetExecute
        {
            get
            {
                Query testQuery1 = new Query(Commands.GET, "USERS:1000", string.Empty);
                QueryResult testQueryResult1 = new QueryResult("GetSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum1 = new KeyValuePair<Query, QueryResult>(testQuery1, testQueryResult1);
                yield return testDatum1;

                Query testQuery2 = new Query(Commands.SET, "ID", "3");
                QueryResult testQueryResult2 = new QueryResult("SetSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum2 = new KeyValuePair<Query, QueryResult>(testQuery2, testQueryResult2);
                yield return testDatum2;

                Query testQuery3 = new Query(Commands.DELETE, "ID", "3");
                QueryResult testQueryResult3 = new QueryResult("DeleteSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum3 = new KeyValuePair<Query, QueryResult>(testQuery3, testQueryResult3);
                yield return testDatum3;

                Query testQuery4 = new Query(Commands.SAVE, "USERS.bin", string.Empty);
                QueryResult testQueryResult4 = new QueryResult("SaveSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum4 = new KeyValuePair<Query, QueryResult>(testQuery4, testQueryResult4);
                yield return testDatum4;

                Query testQuery5 = new Query(Commands.LOAD, "USERS.bin", string.Empty);
                QueryResult testQueryResult5 = new QueryResult("LoadSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum5 = new KeyValuePair<Query, QueryResult>(testQuery5, testQueryResult5);
                yield return testDatum5;


            }

        }

    }
}
