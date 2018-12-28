using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kpable.InGameConsole
{
    public class CommandBuilder
    {
        public static Command Build(string alias, Dictionary<string, object> arguments)
        {
            Callback cb = null;
            Argument[] args = null;
            string description = "";

            if(arguments.ContainsKey("description"))
            {
                description = (string)arguments["description"];
            }

            if (!arguments.ContainsKey("target"))
            {
                Console.Instance.Log.Error("Failed to register <b>" + alias + "</b>. Missing <b>target</b> parameter");
                //return null;
            }

            if(arguments.ContainsKey("args"))
            {
                args = ArgumentBuilder.BuildAll(arguments["args"]);
            }

            return new Command(alias, cb, args, description);
        }
    }
}