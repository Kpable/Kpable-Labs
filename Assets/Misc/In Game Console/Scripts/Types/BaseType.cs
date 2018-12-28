using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

namespace Kpable.InGameConsole
{
    public class BaseType : IBaseType
    {
        Match regexMatch;

        public string Name { get { return Type.ToString(); } }
        public TypeCode Type { get; set; }

        public virtual bool Check(string value)
        {
            Regex regex = Console.Instance.RegEx.Get(Type);
            if(regex != null)
            {
                regexMatch = regex.Match(value);

                return regexMatch.Success;                
            }

            return false;
        }

        public virtual object Get()
        {
            throw new System.NotImplementedException();
        }
    }
}