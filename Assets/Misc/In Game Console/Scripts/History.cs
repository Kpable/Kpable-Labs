using UnityEngine;
using System.Collections.Generic;

public class History
{
    List<string> history;
    int current = -1;

    public void Push(string commnad)
    {

    }

    public string Previous()
    {
        current += 1;
        return "";
    }

    public string Next()
    {
        current -= 1;
        return "";
    }
    public void Reset()
    {
        current = -1;
    }

    public void PrintAll()
    {

    }
}
