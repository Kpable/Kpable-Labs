using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Kpable.InGameConsole
{
    public class ArgumentBuilder
    {
        public static Argument Build(string name, TypeCode type = TypeCode.Empty)
        {
            var argType = TypesBuilder.Build(type);
            return new Argument(name, argType);
        }

        public static List<Argument> BuildAll(Argument[] arguments)
        {
            return null;
        }

        public static Argument[] BuildAll(object v)
        {
            object[] args = (object[])v;

            List<Argument> builtArgs = new List<Argument>();

            if (args.Length > 1)            
                builtArgs.Add(Build(args[0].ToString(), (TypeCode)args[1]));            
            else
                builtArgs.Add(Build(args[0].ToString(), (TypeCode)args[0]));



            return builtArgs.ToArray();
        }
    }
}
