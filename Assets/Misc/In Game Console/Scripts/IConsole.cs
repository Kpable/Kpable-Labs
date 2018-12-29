using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kpable.InGameConsole
{

    public interface IConsole
    {
        

        bool Register(string alias, Dictionary<string, object[]> arguments);
        void Write(string message);
        void WriteLine(string message = "");
        void ToggleConsole();

    }
}