using System;

namespace NoiseDB
{
    /// <summary>
    /// This class represents any given transaction
    /// to be executed
    /// against the Key-Value store.
    /// 
    /// </summary>
    public class Query
    {
        //The Command to execute (get, set, connect,etc)
        public Commands Command { get; private set;}        
        public string Key { get; private set;}        
        public string Argument { get; private set;}
        //Stored as a field so that the time is more accurate and won't
        //depend on when ToString() is called.
        public DateTime ExecutionDate { get; private set; }

        /// <summary>
        /// Simple constructor that initializes all
        /// the fields.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="key"></param>
        /// <param name="argument"></param>
        public Query(Commands command, string key, string argument)
        {
            this.Command = command;
            this.Key = key;
            this.Argument = argument;
            ExecutionDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Command.ToString() + " " + Key?.ToString() + " " + Argument?.ToString() + " at " + ExecutionDate;
        }
    }
}
