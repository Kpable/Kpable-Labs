using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kpable.Mechanics;

public class Projectile : MonoBehaviour, IPooledObject {

    [SerializeField]
    private WeaponType _type;

    public WeaponType type
    {
        get
        {
            return _type;
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        
    }

    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        GetComponent<Renderer>().material.color = def.projectileColor;

    }

    public void OnObjectSpawned()
    {
        // ;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(name + " hit something:" + collider.name);

        Health health = collider.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.Damage((int)Main.W_DEFS[_type].damageOnHit);
            gameObject.SetActive(false);
        }

    }
}
