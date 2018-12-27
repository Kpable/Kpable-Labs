using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Kpable.InGameConsole
{

    public class BaseCommands
    {
        public BaseCommands()
        {
            Initialize();
        }

        public void Initialize()
        {
            Console.Instance.Register
                ("echo", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("history", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("commands", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("help", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("quit", new Dictionary<string, object>()
                {
                    { "description" , "Exits application" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("clear", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("version", new Dictionary<string, object>()
                {
                    { "description" , "Prints a string in console" },
                    { "args", typeof(string) },
                    { "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });
            
        }

    }

}