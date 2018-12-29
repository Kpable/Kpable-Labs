using UnityEngine;
using System.Collections.Generic;

namespace Kpable.InGameConsole
{
    public interface ICommands
    {
        bool Register(string alias, Dictionary<string, object[]> arguments);
        void Deregister(string alias);
        void PrintAll();
        Command Get(string alias);
        bool Has(string alias);
    }
}
