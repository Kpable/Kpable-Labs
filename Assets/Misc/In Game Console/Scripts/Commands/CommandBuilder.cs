using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class CommandBuilder
    {

        public static Command Build(string alias, params string[] arguments)
        {
            Dictionary<string, object> args = arguments.ToDictionary(k => k, v => (object)v);

            if (!args.ContainsKey("target") && args["target"] != null)
            {
                Console.Instance.Log.Error("Failed to register [b]" + alias + "[/b]. Missing [b]target[/b] parameter");
                return null;
            }

            if(args.ContainsKey("args"))
            {
                ArgumentBuilder.BuildAll(args["args"]);
            }

            return null;
        }
    }
}