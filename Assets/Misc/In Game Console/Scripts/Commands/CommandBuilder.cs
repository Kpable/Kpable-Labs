using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;


namespace Kpable.InGameConsole
{
    public class CommandBuilder
    {
        public static Command Build(string alias, Dictionary<string, object[]> arguments)
        {
            Callback cb = null;
            Argument[] args = null;
            string description = "";

            if(arguments.ContainsKey("description"))
            {
                description = (string)arguments["description"][0];
            }

            if (!arguments.ContainsKey("target"))
            {
                Console.Instance.Log.Error("Failed to register <b>" + alias + "</b>. Missing <b>target</b> parameter");
                //return null;
            }
            else
            {
                cb = new Callback(arguments["target"]);
                //MethodInfo mi = (MethodInfo)arguments["target"];
                //cb = new Callback(mi);
            }

            if (arguments.ContainsKey("args"))
            {
                args = ArgumentBuilder.BuildAll(arguments["args"]);
            }

            return new Command(alias, cb, args, description);
        }
    }
}