using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public WeaponDefinition[] weaponDefinitions;

    void Awake()
    {
        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (W_DEFS.ContainsKey(wt))
        {
            return W_DEFS[wt];
        }

        // failed to find the WeaponDefintion
        return new WeaponDefinition();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
