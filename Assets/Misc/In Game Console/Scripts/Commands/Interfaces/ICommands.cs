using UnityEngine;
using System.Collections;

namespace Kpable.InGameConsole
{
    public interface ICommands
    {
        int Register(string alias, params string[] arguments);
        void Deregister(string alias);
        void PrintAll();
        Command Get(string alias);
        bool Has(string alias);
    }
}
