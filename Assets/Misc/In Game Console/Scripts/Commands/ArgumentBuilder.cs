using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArgumentBuilder
{
    Argument argument;

    static Argument Build(string name, int type)
    {
        return null;
    }

    static List<Argument> BuildAll(Argument[] arguments)
    {
        return null;
    }

    public static void BuildAll(object v)
    {
        var args = (ValueTuple) v;
        
    }
}
