using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Kpable.InGameConsole
{
    public class StringType : BaseType
    {

        string Value { get; set; } 

        public StringType()
        {
            Type = TypeCode.String;
        }

        public override bool Check(string value)
        {
            Value = value;
            return true;
        }

        public override object Get()
        {
            return Value;
        }

    }
}