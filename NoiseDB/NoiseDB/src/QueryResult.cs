using System;

namespace NoiseDB
{
    /// <summary>
    /// A class that represents the result
    /// of any given executed Query object.
    /// </summary>
    public class QueryResult
    {
        //If an exception was thrown at any point because
        //of the executed query, it is stored here.
        public Exception ThrownException { get; set; }
        //Whether the Query succeeded or failed.
        public string ResultMessage { get; set; }
        //The data that executed Query retrieved (if any).
        public string RetrievedData { get; set; }
        
        /// <summary>
        /// A simple constructor that initializes all fields.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="thrownException"></param>
        /// <param name="retrievedData"></param>
        public QueryResult(string message, Exception thrownException, string retrievedData)
        {
            ThrownException = thrownException;
            ResultMessage = message;
            RetrievedData = retrievedData;
        }
        
        /// <summary>
        /// Pretty prints a QueryResult into an english representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(ThrownException == null)
            {
                return ResultMessage + " " + RetrievedData;
            }
            else
            {
                return ResultMessage + " with Exception : " + ThrownException.ToString();
            }
        }

    }
}
