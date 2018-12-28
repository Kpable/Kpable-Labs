using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.InGameConsole
{
    public interface ICommand
    {        

        bool Run(string[] args);
        void Describe();
        bool RequireArgs();
        bool RequireStrings();
    }
}