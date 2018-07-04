using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Utilities;

namespace Kpable.Shmup {

    public class Laser : Projectile {

        float beamlength;
        LineRenderer lineRenderer;
        Rigidbody2D body;

        // Use this for initialization
        void Start() {
            body = GetComponent<Rigidbody2D>();
            body.velocity = Vector3.up * WeaponManager.GetWeaponDefinition(Type).velocity;
        }

        // Update is called once per frame
        void Update() {

        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log(name + " hit something:" + collision.name);

            ReflectiveSurface reflectiveSurface = collision.gameObject.GetComponent<ReflectiveSurface>();
            if (reflectiveSurface != null)
            {
                if (reflectiveSurface.fixedReflection)
                {
                    Debug.Log("Velocity " + body.velocity);
                    body.velocity = WeaponManager.GetWeaponDefinition(Type).velocity * reflectiveSurface.reflectDirection.Vec();
                    Debug.Log("Velocity " + body.velocity);

                }


            }
            base.OnTriggerEnter2D(collision);
        }
    }

}