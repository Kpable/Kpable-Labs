using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.Shmup
{

    public class PowerUp : MonoBehaviour
    {
        public WeaponType weaponType = WeaponType.blaster;
        // Use this for initialization
        void Start()
        {
            GetComponent<SpriteRenderer>().color = WeaponManager.W_DEFS[weaponType].color;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}