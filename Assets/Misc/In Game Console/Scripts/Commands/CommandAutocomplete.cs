using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class CommandAutocomplete
    {
        string filter = null;
        public List<string> Filtered = new List<string>();
        int current = -1;

        public string Filter(string filter)
        {
            if (this.filter == filter)
                return null;

            var willBeFiltered = Console.Instance.Commands.CommandsSet.Keys.ToArray();

            if (!string.IsNullOrEmpty(filter) &&
                this.filter != null && 
                filter.Length > this.filter.Length &&
                filter.StartsWith(this.filter))

                willBeFiltered = Filtered.ToArray();

            this.filter = filter;

            foreach (var command in Console.Instance.Commands.CommandsSet.Keys.ToArray())
            {
                if (command.StartsWith(filter))
                    Filtered.Add(command);
            }
            return null;
        }

        public string Next()
        {
            if (Filtered.Count > 0)
            {
                if (current == Filtered.Count - 1)
                    current = -1;

                current += 1;
            }

            return Filtered[current];
        }

        public void Reset()
        {
            this.filter = null;
            Filtered.Clear();
            current = -1;
        }
    }
}
