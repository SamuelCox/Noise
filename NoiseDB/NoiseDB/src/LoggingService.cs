using System;
using System.Configuration;
using System.IO;

namespace NoiseDB
{
    /// <summary>
    /// A simple static class that handles logging transactions to log files.
    /// </summary>
    internal static class LoggingService
    {
        public const string DATE_FORMAT = "yyyy-MM-dd";
        

        /// <summary>
        /// A method that takes a Query, and writes a log of that Query and when it was
        /// executed to a file on disk. All Queries executed on a given day are written
        /// to a file with that date as the filename. There's not a massive amount
        /// of complex logic here, just StreamWriters.
        /// </summary>
        /// <param name="query">The Query object to log.</param>
        /// <param name="remoteQuery">Whether or not this was a query processed on the server.</param>
        internal static void LogToDisk(Query query, bool remoteQuery)
        {
            string directoryPath = ConfigurationManager.AppSettings["LoggingDirectory"];
            string queryLogString;
            if (remoteQuery)
            {
                queryLogString = query.ToString() + " processed on server";
                
            }
            else
            {
                queryLogString = query.ToString();
            }
            
            if(Directory.Exists(directoryPath))
            {
                DateTime currentDay = DateTime.Today.Date;
                if (File.Exists(directoryPath + @"\" + currentDay.ToString(DATE_FORMAT) + ".txt"))
                {

                    using (StreamWriter fileWriter = new StreamWriter(directoryPath + @"\" + currentDay.ToString(DATE_FORMAT) + ".txt", true))
                    {
                        fileWriter.WriteLine(queryLogString);
                    }
                }
                else 
                {                                        
                    using (StreamWriter fileWriter = new StreamWriter(directoryPath + @"\" + currentDay.ToString(DATE_FORMAT) + ".txt", false))
                    {
                        fileWriter.WriteLine(queryLogString);
                    }                                                            
                }
            }
        }
    }
}
