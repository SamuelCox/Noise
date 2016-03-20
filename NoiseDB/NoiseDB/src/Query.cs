﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseDB
{
    public class Query
    {
        public Commands Command { get; private set;}
        public string Key { get; private set;}
        public string Argument { get; private set;}
        public DateTime ExecutionDate { get; private set; }

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
