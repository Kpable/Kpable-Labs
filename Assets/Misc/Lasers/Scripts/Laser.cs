using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Utilities;

namespace Kpable.Shmup {

    [RequireComponent(typeof(LineRenderer))]
    public class Laser : Projectile {

        float beamlength;
        float maxDistance = 20f;
        LineRenderer lineRenderer;
        Rigidbody2D body;

        // Use this for initialization
        void Start() {
            body = GetComponent<Rigidbody2D>();
            body.velocity = Vector3.up * WeaponManager.GetWeaponDefinition(Type).velocity;
            lineRenderer = GetComponent<LineRenderer>();
            //UpdateBeam();
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
                else
                {
                    ContactPoint2D[] points = new ContactPoint2D[10];
                    int pointCounts = collision.GetContacts(points);
                    Debug.Log("points " + pointCounts);
                    //Vector3.Reflect(body.velocity.normalized, );
                }
            }
            base.OnTriggerEnter2D(collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ReflectiveSurface reflectiveSurface = collision.gameObject.GetComponent<ReflectiveSurface>();
            if (reflectiveSurface != null)
            {
                if (reflectiveSurface.fixedReflection)
                {
                    Debug.Log("Velocity " + body.velocity);
                    body.velocity = WeaponManager.GetWeaponDefinition(Type).velocity * reflectiveSurface.reflectDirection.Vec();
                    Debug.Log("Velocity " + body.velocity);
                }
                else
                {

                    ContactPoint2D[] points = new ContactPoint2D[10];
                    int pointCounts = collision.GetContacts(points);
                    Debug.Log("points " + pointCounts);
                    //Vector3.Reflect(body.velocity.normalized, );
                }
            }
        }

        void UpdateBeam()
        {
            RaycastHit2D[] hit;
            int points = 1;
            Vector3 lastHitPoint = transform.position;
            Vector3 dirToFire = transform.up;
            beamlength = 0;
            lineRenderer.SetPosition(0, transform.position);

            hit = Physics2D.RaycastAll(lastHitPoint, dirToFire, maxDistance - beamlength);
            Debug.Log("Hits " + hit.Length);
            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log("Hit " + i + ": " + hit[i].point); 
            }



            for (int i = 0; i < hit.Length; i++)
            {
                // if it hit itself and theres more hits, continue
                if (new Vector3(hit[i].point.x, hit[i].point.y, lastHitPoint.z) == lastHitPoint && hit.Length > 1)
                    continue;
                // else if it hit itself and there is only one hit, it hit nothing
                else if (new Vector3(hit[i].point.x, hit[i].point.y, lastHitPoint.z) == lastHitPoint && hit.Length == 1)
                {
                    beamlength = maxDistance;
                    break;
                }

                // 
                if (hit[i].collider == null || hit.Length == 1)
                {
                    lineRenderer.SetPosition(points, dirToFire * (maxDistance - beamlength));
                    beamlength = maxDistance;
                }
                else
                {
                    beamlength += hit[i].distance;
                    lineRenderer.SetPosition(points, transform.InverseTransformPoint(hit[i].point));
                    points++;
                    lineRenderer.positionCount = points + 1;
                    lastHitPoint = hit[i].point;

                    // get reflection
                    dirToFire = Vector3.Reflect(dirToFire, hit[i].normal);

                }
                    
                
            }
        }
    }

}