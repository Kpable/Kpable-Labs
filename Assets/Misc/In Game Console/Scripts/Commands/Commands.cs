using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Kpable.InGameConsole
{

    public class Commands : ICommands
    {
        public CommandAutocomplete Autocomplete { get; set; }

        Dictionary<string, Command> commands = new Dictionary<string, Command>();
        CommandBuilder commandBuilder;

        public Commands()
        {            
            commandBuilder = new CommandBuilder();
            Autocomplete = new CommandAutocomplete();
        }

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
            foreach (var c in CommandsSet.Values)
            {
                c.Describe();
            }
        }

        public bool Register(string alias, Dictionary<string, object[]> arguments)
        {
            if (CommandsSet.ContainsKey(alias))
            {
                Console.Instance.Log.Warn("Failed to register [b]" + alias + "[/b]. Command already exists.");
                return false;
            }

            Command command = CommandBuilder.Build(alias, arguments);
            if (command != null)
            {                
                CommandsSet.Add(alias, command);
                return true;
            }

            return true;
        }

    }
}
