using UnityEngine;
using System.Collections;

namespace Kpable.InGameConsole
{
    public class Argument : IArgument
    {
        string Name { get; set; }
        public BaseType ArgType { get; set; }

        public Argument(string name, BaseType argType)
        {
            Name = name;
            ArgType = argType;
        }

        public object GetValue()
        {
            return ArgType.Get();
        }

        public bool SetValue(string value)
        {            
            return ArgType.Check(value); ;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}