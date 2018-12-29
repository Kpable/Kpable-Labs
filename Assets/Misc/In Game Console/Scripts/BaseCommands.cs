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
                ("echo", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    { "target", new object[] { Console.Instance, "WriteLine" } }
                    
                });

            Console.Instance.Register
                ("history", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    //{ "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("commands", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    //{ "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Action<string> helpact = new Action<string>(Help);
            
            Console.Instance.Register
                ("help", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] { "Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    { "target", new object[] { this, "Help" } }
                    //{ "target", helpact.Target }
                });

            Console.Instance.Register
                ("quit", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] { "Exits application" } },
                    { "args", new object[] { "text", TypeCode.String } },
                    //{ "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("clear", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    //{ "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });

            Console.Instance.Register
                ("version", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Prints a string in console" } },
                    { "args", new object[] { TypeCode.String } },
                    //{ "target", ValueTuple.Create(Console.Instance, "WriteLine")}
                });
            
        }

        public static void Help(string command = null)
        {
            if (command != null)
            {
                Command c = Console.Instance.Commands.Get(command);
                if(c != null)
                {
                    c.Describe();
                }
                else
                {
                    Console.Instance.Log.Warn("No such comand");
                    return;
                }
            }
            else
            {
                Console.Instance.WriteLine(
                    "Type <#ffff66><link='id_01'>help</link> <command-name></color> show information about command.");
            }


                
        }

    }

}