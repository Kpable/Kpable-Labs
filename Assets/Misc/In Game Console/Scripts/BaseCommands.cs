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
                    { "description" , new object[] {"Prints all previous commands used during the session" } },
                    { "target", new object[] { Console.Instance.History, "PrintAll" } }
                });

            Console.Instance.Register
                ("commands", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Lists all available commands" } },
                    { "target", new object[] { Console.Instance.Commands, "PrintAll" } }
                });
            
            Console.Instance.Register
                ("help", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] { "Outputs usage instructions" } },
                    { "args", new object[] { TypeCode.String } },
                    { "target", new object[] { this, "Help" } }
                });

            Console.Instance.Register
                ("quit", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] { "Exits application" } },
                    { "target", new object[] { this, "Quit" }}
                });

            Console.Instance.Register
                ("clear", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Clears the console" } },
                    { "target", new object[] { this, "Clear" }}
                });

            Console.Instance.Register
                ("version", new Dictionary<string, object[]>()
                {
                    { "description" , new object[] {"Shows the version" } },
                    { "target", new object[] { this, "Version" }}
                });
            
        }

        public static void Help(string command = null)
        {
            if (!string.IsNullOrEmpty(command))
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