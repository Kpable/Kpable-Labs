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
            history.Add(command);
        }

        public string Previous()
        {
            current += 1;
            return "";
        }

        public string Next()
        {
            current -= 1;
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