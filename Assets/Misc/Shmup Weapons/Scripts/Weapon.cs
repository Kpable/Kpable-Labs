using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Mechanics;
using System;

namespace Kpable.Shmup
{
    public enum WeaponType
    {
        none,
        blaster,
        spread,
        phaser,
        missle,
        laser,
        shield
    }

    [System.Serializable]
    public class WeaponDefinition
    {
        public WeaponType type = WeaponType.none;
        public string letter;
        public Color color = Color.white;
        public GameObject projectilePrefab;
        public Color projectileColor = Color.white;
        public float damageOnHit = 0;
        public float continuousDamage = 0;
        public float delayBetweenShots = 0;
        public float velocity = 20;
        public int guns = 1;
    }

    public class Weapon : MonoBehaviour
    {

        public WeaponType weaponType = WeaponType.blaster;
        [HideInInspector]
        public WeaponDefinition def;
        public GameObject collar;
        public float lastShot;
        public int guns = 1;
        public float gunRadius = 1;
        
       

        List<Transform> gunObjects;
        ObjectPooler objectPooler;
        // Use this for initialization
        void Start()
        {
            def = WeaponManager.GetWeaponDefinition(weaponType);
            objectPooler = ObjectPooler.Instance;
            gunObjects = new List<Transform>();            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Jump") == 1)
                Fire();
        }

        public void Fire()
        {
            if (!gameObject.activeInHierarchy) return;
            if (Time.time - lastShot < def.delayBetweenShots) return;

            if (gunObjects.Count < def.guns)
            {
                for (int i = gunObjects.Count; i < def.guns; i++)
                {
                    GameObject gunObject = new GameObject("Gun");
                    gunObject.transform.SetParent(transform);
                    gunObjects.Add(gunObject.transform);

                }
                PositionGuns();
            }


            Projectile p;
            switch (weaponType)
            {
                case WeaponType.blaster:
                    p = MakeProjectile();
                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;
                    break;
                case WeaponType.spread:
                    

                    p = MakeProjectile();
                    p.transform.position = gunObjects[0].position;
                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;

                    p = MakeProjectile();
                    p.transform.position = gunObjects[1].position;
                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;

                    p = MakeProjectile();
                    p.transform.position = gunObjects[2].position;

                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;
                    break;
            }
        }

        private void PositionGuns()
        {
            for (int i = 0; i < gunObjects.Count; i++)
            {
                gunObjects[i].position = GetPositionInCircle(transform.position, (i * 45) + 45, gunRadius);
            }
        }

        public Projectile MakeProjectile()
        {
            //GameObject go = Instantiate(def.projectilePrefab);
            GameObject go = objectPooler.SpawnFromPool("projectile");


            go.transform.position = collar.transform.position;
            //go.transform.parent = PROJECTILE_ANCHOR;
            Projectile p = go.GetComponent<Projectile>();
            p.type = weaponType;
            lastShot = Time.time;
            return (p);
        }

        Vector3 GetPositionInCircle(Vector3 origin, float angle, float radius)
        {
            Vector3 pos = Vector3.zero;
            pos.x = origin.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            pos.y = origin.y + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            return pos;
        }
    }


}