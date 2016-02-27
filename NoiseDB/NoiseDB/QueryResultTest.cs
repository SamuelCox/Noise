using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NoiseDB
{
    [TestFixture]
    class QueryResultTest
    {
        public QueryResultTest()
        {

        }

        [TestCaseSource("GetQueryResults")]
        [Test]
        public void TestToString(QueryResult result)
        {
            string queryResultToString = result.ToString();
            if(result.ResultMessage == "Success")
            {
                if(result.RetrievedData != null)
                {
                    string expectedString = "Success " + result.RetrievedData.ToString();
                    
                    Assert.AreEqual(expectedString, queryResultToString);
                }
            }
            else
            {
                if(result.ThrownException != null)
                {
                    string expectedString = "Failed with Exception : " + result.ThrownException.ToString();
                    
                    Assert.AreEqual(expectedString, queryResultToString);
                }
                else
                {
                    string expectedString = "Failed ";
                    Assert.AreEqual(expectedString, queryResultToString);
                    
                }
            }
        }

        public IEnumerable<QueryResult> GetQueryResults
        {
            get
            {
                List<string> testResultData = new List<string>();
                testResultData.Add("3");
                testResultData.Add("4");
                yield return new QueryResult("Success",null,null);
                yield return new QueryResult("Success", null, testResultData);
                yield return new QueryResult("Failed", new Exception("test"), null);
                yield return new QueryResult("Failed", null, null);
            }
        }

    }
}
