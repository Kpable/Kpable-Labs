using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class CommandAutocomplete
    {
        string filter = null;
        List<string> filtered = new List<string>();
        int current = -1;

        public string Filter(string filter)
        {
            if (this.filter == filter)
                return null;

            var willBeFiltered = Console.Instance.Commands.CommandsSet.Keys.ToArray();

            if (!string.IsNullOrEmpty(filter) &&
                filter.Length > this.filter.Length &&
                filter.StartsWith(this.filter))

                willBeFiltered = this.filtered.ToArray();

            this.filter = filter;

            foreach (var command in Console.Instance.Commands.CommandsSet.Keys.ToArray())
            {
                if (command.StartsWith(filter))
                    filtered.Add(command);
            }
            return null;
        }

        public string Next()
        {
            if (filtered.Count > 0)
            {
                if (current == filtered.Count - 1)
                    current = -1;

                current += 1;
            }

            return filtered[current];
        }

        public void Reset()
        {
            this.filter = null;
            filtered.Clear();
            current = -1;
        }
    }
}
