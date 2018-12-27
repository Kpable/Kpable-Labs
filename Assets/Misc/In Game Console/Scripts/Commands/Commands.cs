using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Kpable.InGameConsole
{
    
    public class Commands : ICommands
    {
        CommandAutocomplete autocomplete;
        public CommandAutocomplete Autocomplete { get { return autocomplete; } } 

        Dictionary<string, Command> commands;
        CommandBuilder commandBuilder;

        public Dictionary<string, Command> CommandsSet { get { return commands; } }

        public void Deregister(string alias)
        {
            if (CommandsSet.ContainsKey(alias))
                CommandsSet.Remove(alias);
        }

        public Command Get(string alias)
        {
            if (CommandsSet.ContainsKey(alias))
                return CommandsSet[alias];

            // Try autocomplete
            foreach (var c in CommandsSet)
            {
                if (c.Key.StartsWith(alias))
                    return CommandsSet[c.Key];
            }

            return null;
        }

        public bool Has(string alias)
        {
            return CommandsSet.ContainsKey(alias);
        }

        public void PrintAll()
        {
            throw new System.NotImplementedException();
        }

        public int Register(string alias, params string[] arguments)
        {
            if (CommandsSet.ContainsKey(alias))
            {
                Console.Instance.Log.Warn("Failed to register [b]" + alias + "[/b]. Command already exists.");
                return 1;
            }

            Command command = CommandBuilder.Build(alias, arguments);
            if (command)
            {                
                CommandsSet.Add(alias, command);
                return 0;
            }

            return 0;
        }
    }
}
