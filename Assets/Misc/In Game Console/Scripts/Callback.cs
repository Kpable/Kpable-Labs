using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Kpable.InGameConsole
{
    public class Callback
    {
        object Target;
        MethodInfo Method;

        public Callback(object[] target)
        {
            Target = target[0];
            Type targetType = Target.GetType();
            string methodName = (string)target[1];
            Method = targetType.GetMethod(methodName);
        }

        public void Call(List<Argument> args)
        {
            
            if (args.Count == 1)
                Method.Invoke(Target, new object[] { args[0].GetValue() });
            else
            {
                object[] arguments = new object[args.Count];

                for (int i = 0; i < args.Count; i++)
                {
                    arguments[i] = args[i].GetValue();
                }

                Method.Invoke(Target, arguments);
            }
        }
    }
}