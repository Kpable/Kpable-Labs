using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class Weapon : MonoBehaviour {

    public WeaponType _type = WeaponType.blaster;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShot;

    ObjectPooler objectPooler;
    // Use this for initialization
    void Start () {
        def = Main.GetWeaponDefinition(_type);
        objectPooler = ObjectPooler.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Jump") == 1)
            Fire();
	}

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Time.time - lastShot < def.delayBetweenShots) return;

        Projectile p;
        switch (_type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.transform.rotation = transform.rotation;
                p.GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector3.up * def.velocity;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.GetComponent<Rigidbody2D>().velocity = Vector3.up * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody2D>().velocity = new Vector3(-0.2f, 0.9f, 0) * def.velocity;
                p = MakeProjectile();
                p.GetComponent<Rigidbody2D>().velocity = new Vector3(0.2f, 0.9f, 0) * def.velocity;
                break;
        }
    }

    public Projectile MakeProjectile()
    {
        //GameObject go = Instantiate(def.projectilePrefab);
        GameObject go = objectPooler.SpawnFromPool("projectile");

        go.transform.position = collar.transform.position;
        //go.transform.parent = PROJECTILE_ANCHOR;
        Projectile p = go.GetComponent<Projectile>();
        p.type = _type;
        lastShot = Time.time;
        return (p);
    }
}
