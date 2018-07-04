using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.Shmup {
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceshipBehavior : MonoBehaviour {

        public float thrust = 250f;
        public float rotationSpeed = 180f;
        public float drag = 0.8f;
        public float gravityScale = 0f;

        private float hAxis, vAxis;

        Rigidbody2D body;

        Weapon weapon; 

        // Use this for initialization
        void Start() {
            body = GetComponent<Rigidbody2D>();
            body.drag = drag;
            body.gravityScale = gravityScale;

            weapon = GetComponent<Weapon>();
        }

        // Update is called once per frame
        void Update() {
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {

            Rotation();
            Thrust();
        }

        private void Rotation()
        {
            Quaternion currentRotation = transform.rotation;

            float z = currentRotation.eulerAngles.z;
            z += -hAxis * rotationSpeed * Time.deltaTime;

            currentRotation = Quaternion.Euler(0, 0, z);

            transform.rotation = currentRotation;
        }

        private void Thrust()
        {
            Vector3 force = new Vector3(0, vAxis * thrust * Time.deltaTime, 0);

            body.AddForce(transform.rotation * force);

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            var col = collision.GetComponent<PowerUp>();
            if ( col != null)
            {
                weapon.WeaponType = col.weaponType;
            }
    }
    }

}