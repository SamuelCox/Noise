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

        
        [Test]
        public void TestConstructQuery()
        {
            QueryService queryService = new QueryService();
            foreach(KeyValuePair<string, Query> kvp in GetConstructTestCases)
            {
                Query query = queryService.ConstructQuery(kvp.Key);
                Assert.AreEqual(kvp.Value.Command, query.Command);
                Assert.AreEqual(kvp.Value.Argument, query.Argument);
                Assert.AreEqual(kvp.Value.Key, query.Key);
            }
            
            
            
        }

        
        [Test]
        public void TestExecuteQuery()
        {
            
            List<KeyValuePair<Query, QueryResult>> queryTestData = GetExecuteTestCases.ToList();
            foreach (KeyValuePair<Query, QueryResult> keyValuePair in queryTestData)
            {
                QueryService queryService = new QueryService(new MockDataService(), new MockQueryTcpClient(),
                                                         new MockQueryTcpServer());

                QueryResult queryResult = queryService.ExecuteQuery(keyValuePair.Key);
                Assert.AreEqual(keyValuePair.Value.ResultMessage, queryResult.ResultMessage);
            }

        }

        public IEnumerable<KeyValuePair<string, Query>> GetConstructTestCases
        {
            get
            {
                yield return new KeyValuePair<string, Query>(@"GET ""X""", new Query(Commands.GET, "X", string.Empty));

                yield return new KeyValuePair<string, Query>(@"SET ""X"" ""Y""", new Query(Commands.SET, "X", "Y"));

                yield return new KeyValuePair<string, Query>(@"SET ""X X X"" ""Y Y Y""" , new Query(Commands.SET, "X X X", "Y Y Y"));

                yield return new KeyValuePair<string, Query>(@"SERVER_CONNECT ""127.0.0.1""", new Query(Commands.SERVER_CONNECT, "127.0.0.1", string.Empty));


            }
        }

        public IEnumerable<KeyValuePair<Query, QueryResult>> GetExecuteTestCases
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

                Query testQuery6 = new Query(Commands.SERVER_CONNECT, "127.0.0.1", string.Empty);
                QueryResult testQueryResult6 = new QueryResult("ConnectSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum6 = new KeyValuePair<Query, QueryResult>(testQuery6, testQueryResult6);
                yield return testDatum6;

                Query testQuery7 = new Query(Commands.SERVER_DISCONNECT, string.Empty, string.Empty);
                QueryResult testQueryResult7 = new QueryResult("Success", null, null);
                KeyValuePair<Query, QueryResult> testDatum7 = new KeyValuePair<Query, QueryResult>(testQuery7, testQueryResult7);
                yield return testDatum7;

                Query testQuery8 = new Query(Commands.SERVER_START, string.Empty, string.Empty);
                QueryResult testQueryResult8 = new QueryResult("ListenSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum8 = new KeyValuePair<Query, QueryResult>(testQuery8, testQueryResult8);
                yield return testDatum8;

                Query testQuery9 = new Query(Commands.SERVER_STOP, string.Empty, string.Empty);
                QueryResult testQueryResult9 = new QueryResult("StopListenSuccess", null, null);
                KeyValuePair<Query, QueryResult> testDatum9 = new KeyValuePair<Query, QueryResult>(testQuery9, testQueryResult9);
                yield return testDatum9;

                Query testQuery10 = new Query(Commands.UNKNOWN, string.Empty, string.Empty);
                QueryResult testQueryResult10 = new QueryResult("Unrecognised Command", null, null);
                KeyValuePair<Query, QueryResult> testDatum10 = new KeyValuePair<Query, QueryResult>(testQuery10, testQueryResult10);
                yield return testDatum10;



            }

        }

    }
}
