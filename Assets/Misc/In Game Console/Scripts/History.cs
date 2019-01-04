using UnityEngine;
using System.Collections.Generic;

namespace Kpable.InGameConsole
{
    public class History
    {
        List<string> history = new List<string>();
        int current = -1;

        public void Push(string command)
        {
            // if this command is the same as the last one, return early
            if (history.Count > 0 && history[history.Count - 1] == command)
                return;

            history.Add(command);
        }

        public string Previous()
        {
            if(history.Count > 0 && current < history.Count -1)
            {
                current++;
                return history[history.Count - current - 1];                
            }

            return "";
        }

        public string Next()
        {
            if (history.Count > 0 && current > 0)
            {
                current--;
                return history[history.Count - current - 1];
            }
            else
                Reset();

            return "";
        }

        public void Reset()
        {
            current = -1;
        }

        public void PrintAll()
        {
            for (int i = 0; i < history.Count; i++)
            {
                Console.Instance.WriteLine("<b>" + (i + 1) + "</b> <#ffff66><link='id_01'>" + history[i] + "</link></color>");
            }
        }
    }
}