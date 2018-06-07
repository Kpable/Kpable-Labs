using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Mechanics;


public enum CurrencySyntax { Scientific_Notation, SI, Letters, Total }

/// <summary>
/// Class to handle really big numbers. 
/// 
/// Feature - Currency type - eventually these numbers will start getting huge, need a way(s) to display them
///     Reqs:
///     Display full number until million 0 - 999,999. Then display truncated with suffix, 1.25M 
///     ? Make significant digits customizable?
///     idle oil tycoon uses both scientific notation (3.18E7) or compounded SI unit suffixes (K, M, G, T, P, E, Z, Y, KY, ... YY, KYY)
///     endless frontier saga uses letter notation, a, b, c, ..aa, ab, ac
///     
/// </summary>
public class Currency : ObservedValue<float>
{
    CurrencySyntax unitSyntax = CurrencySyntax.Scientific_Notation;

    public CurrencySyntax UnitSyntax { set { unitSyntax = value; } }
    // need the main value , exponential 
    // 
    public override float Value
    {
        get
        {
            return base.Value;
        }

        set
        {

            base.Value = value;
            if(base.Value >= 1000000)
            {

            }
        }
    }

    public int exponential;

    // Emtpy Constructor
    public Currency() : base()
    {
        
    }

    // Initial Value Constructor
    public Currency(float value) : base(value)
    {

    }

    public override string ToString()
    {
        // change string based on unit syntax
        return base.ToString();
    }
}
