﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class QueryResult
    {
        public Exception ThrownException { get; private set; }
        public string ResultMessage { get; private set; }
        public List<string> RetrievedData { get; private set; }
        
        public QueryResult(string message,Exception thrownException, List<string> retrievedData)
        {
            ThrownException = thrownException;
            ResultMessage = message;
            RetrievedData = retrievedData;
        }

    }
}