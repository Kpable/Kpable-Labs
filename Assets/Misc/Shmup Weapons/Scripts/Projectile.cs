using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Mechanics;

namespace Kpable.Shmup
{
    public class Projectile : MonoBehaviour, IPooledObject
    {

        [SerializeField]
        private WeaponType type;

        public WeaponType Type { get { return type; } set { SetType(value); } }

        public void SetType(WeaponType eType)
        {
            type = eType;
            WeaponDefinition def = WeaponManager.GetWeaponDefinition(type);
            GetComponent<Renderer>().material.color = def.projectileColor;

        }

        public void OnObjectSpawned()
        {
            // ;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(name + " hit something:" + collider.name);

            Health health = collider.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Damage((int)WeaponManager.W_DEFS[type].damageOnHit);
                gameObject.SetActive(false);
            }

        }
    }
}