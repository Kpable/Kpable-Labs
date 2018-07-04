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
        triple,
        phaser,
        missle,
        laser,
        shield
    }

    [System.Serializable]
    public class WeaponDefinition
    {
        public WeaponType type = WeaponType.none;
        public string tag;
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

        [SerializeField]
        WeaponType weaponType = WeaponType.blaster;

        public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; def = WeaponManager.GetWeaponDefinition(weaponType); CreateGuns(); } }
        public float gunRadius = 1f;
        public float angleBetweenGuns = 15f;
        public float angleOffset = 90f;
        public GameObject gunPrefab;

        WeaponDefinition def;
        float lastShot;
        
       

        List<Transform> gunObjects;
        ObjectPooler objectPooler;
        // Use this for initialization
        void Start()
        {
            def = WeaponManager.GetWeaponDefinition(weaponType);
            objectPooler = ObjectPooler.Instance;
            gunObjects = new List<Transform>();

            CreateGuns();
            Debug.Log("Angle " + Vector3.Angle(Vector3.right, transform.rotation.eulerAngles));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Jump") == 1)
                Fire();

            // Dev keys
            if (Input.GetKeyDown(KeyCode.Z))
                AddGun();
            if (Input.GetKeyDown(KeyCode.X))
                RemoveGun();
            if (Input.GetKeyDown(KeyCode.C))
                RefreshGun();


        }

        public void Fire()
        {
            if (!gameObject.activeInHierarchy) return;
            if (Time.time - lastShot < def.delayBetweenShots) return;


            Projectile p;
            switch (weaponType)
            {
                case WeaponType.blaster:
                    p = MakeProjectile();
                    p.transform.position = gunObjects[0].position;
                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;
                    break;
                case WeaponType.triple:
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
                case WeaponType.laser:
                    p = MakeProjectile();
                    p.transform.position = gunObjects[0].position;
                    p.transform.rotation = transform.rotation;
                    p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;
                    break;
            }
        }

        void AddGun()
        {
            GameObject gunObject = Instantiate(gunPrefab);
            gunObject.transform.SetParent(transform);
            gunObjects.Add(gunObject.transform);
            RefreshGun();
        }
        void RemoveGun()
        {
            Transform go = gunObjects[gunObjects.Count - 1];
            gunObjects.RemoveAt(gunObjects.Count - 1);
            Destroy(go.gameObject);
            RefreshGun();

        }
        void RefreshGun()
        {
            //for (int i = 0; i < gunObjects.Count; i++)
            //{
            //    RemoveGun();
            //}
            //CreateGuns();
            PositionGuns();
        }

        private void CreateGuns()
        {
            if (gunObjects.Count < def.guns)
            {
                for (int i = gunObjects.Count; i < def.guns; i++)
                {
                    GameObject gunObject = Instantiate(gunPrefab);
                    gunObject.transform.SetParent(transform);
                    gunObject.transform.rotation = Quaternion.identity;
                    gunObjects.Add(gunObject.transform);
                }

                PositionGuns();
            }
            else if(gunObjects.Count > def.guns)
            {
                for (int i = 0; i < gunObjects.Count; i++)
                {
                    if (i <= def.guns - 1)
                        continue;
                    gunObjects[i].gameObject.SetActive(false);
                }
            }
        }

        private void PositionGuns()
        {
            for (int i = 0; i < gunObjects.Count; i++)
            {
                float angle = 0;
                angle += i * angleBetweenGuns;        // offset angle of gun[i]
                angle *= ((i % 2 == 0) ? 1 : -1);     // let the guns position in alternating sides
                angle += angleOffset;                 // right = 0 on unit circle so offset starting axis
                angle += (i % 2 == 0) ? 0 : -angleBetweenGuns;  // 
                angle += (gunObjects.Count % 2 == 0) ? angleBetweenGuns : 0; // if there are even number of guns space evenly from center
                angle += transform.rotation.eulerAngles.z;  // account for current object's rotation

                gunObjects[i].position = GetPositionInCircle(transform.position, angle, gunRadius);
            }
        }

        public Projectile MakeProjectile()
        {
            GameObject go = objectPooler.SpawnFromPool(def.tag);
            if(go == null)
                go = Instantiate(def.projectilePrefab);
            
            //go.transform.parent = PROJECTILE_ANCHOR;
            Projectile p = go.GetComponent<Projectile>();
            p.Type = weaponType;
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