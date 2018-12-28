using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class RegExLib 
{
    //   patterns = {
    //   '1': '^(1|0|true|false)$',  # bool
    //'2': '^\\d+$',  # int
    //'3': '^[+-]?([0-9]*[\\.\\,]?[0-9]+|[0-9]+[\\.\\,]?[0-9]*)([eE][+-]?[0-9]+)?$',  # float
    //   }

    Dictionary<TypeCode, Regex> Patterns = new Dictionary<TypeCode, Regex>
    {
        { TypeCode.Boolean, new Regex("^(1|0|true|false)$") },
        { TypeCode.Int32, new Regex("^\\d+$") },
        { TypeCode.Decimal, new Regex("^[+-]?([0-9]*[\\.\\,]?[0-9]+|[0-9]+[\\.\\,]?[0-9]*)([eE][+-]?[0-9]+)?$") }
    };

    public Regex Get(TypeCode type)
    {
        if (Patterns.ContainsKey(type))
            return Patterns[type];

        return null;
    }
}
