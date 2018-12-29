using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.InGameConsole
{
    public class Command : ICommand
    {
        public string Alias { get; set; }
        public int RequiredArgumentsCount { get { return (Arguments != null ) ? Arguments.Length : 0; } }

        Callback Target { get; set; }
        Argument[] Arguments { get; set; }
        string Description { get; set; }
        
        public Command(string alias, Callback callback, Argument[] arguments, string description = "")
        {
            Alias = alias;
            Target = callback;
            Arguments = arguments;
            if (string.IsNullOrEmpty(description))
                Console.Instance.Log.Info("No description provided for [b]" + alias + "[/b] command");
            else
                Description = description;
        }

        public bool Run(string[] args)
        {
            List<Argument> arguments = new List<Argument>();

            if (Arguments != null)
            {
                if (args.Length != Arguments.Length)
                {
                    Console.Instance.Log.Warn("Recieved more arguments than command takes");
                }


                for (int i = 0; i < Arguments.Length; i++)
                {
                    bool argumentAssigned = Arguments[i].SetValue(args[i]);

                    if (argumentAssigned)
                        arguments.Add(Arguments[i]);
                    else
                    {
                        Console.Instance.Log.Warn("Argument " + args[i] + ": expected " + Arguments[i].ArgType.Type.ToString());
                        return false;
                    }
                }
            }

            if (Target != null)
                Target.Call(arguments);

            return true;
            
        }

        public void Describe()
        {
            Console.Instance.Write("<#ffff66><link='id_01'>" + Alias + "</link></color>");

            if (RequiredArgumentsCount > 0)
            {
                foreach (var a in Arguments)
                {
                    Console.Instance.Write(" <#88ffff>" + a.ToString() + "</color>");
                }
            }

            if(!string.IsNullOrEmpty(Description))
                Console.Instance.Write(" - " + Description);
            
            Console.Instance.WriteLine();
        }

        public bool RequireArgs()
        {
            if(Arguments != null) return Arguments.Length > 0;
            return false;
        }

        public bool RequireStrings()
        {
            foreach (var a in Arguments)
            {
                if (a.ArgType.Type == System.TypeCode.String) return true;
            }

            return false;
        }
    }
}