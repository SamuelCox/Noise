using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB.src
{
    internal static class LoggingService
    {
        public const string DATE_FORMAT = "yyyy-MM-dd";
        

        internal static void LogToDisk(Query query, bool remoteQuery = false)
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
                        fileWriter.WriteLine(query.ToString());
                    }
                }
                else 
                {
                    File.Create(directoryPath + @"\" + currentDay.ToString(DATE_FORMAT) + ".txt");
                    using (StreamWriter fileWriter = new StreamWriter(directoryPath + @"\" + currentDay.ToString(DATE_FORMAT) + ".txt", false))
                    {
                        fileWriter.WriteLine(query.ToString());
                    }                                                            
                }
            }
        }
    }
}
