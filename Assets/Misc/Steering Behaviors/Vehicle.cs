using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering
{
    public class Vehicle : MovingEntity
    {         
        [SerializeField]
        SteeringBehavior steering;

        public Vector3 target;
        public Transform targetTransform;
        public Vehicle targetVehicle;

        protected override void Awake()
        {
            base.Awake();
            steering = new SteeringBehavior(this);

        }
        // Use this for initialization
        void Start()
        {
            //Position = transform.position;
            //Velocity = Vector3.zero;
            //heading = Velocity;
            //maxSpeed = 15f;
            //maxForce = 2f;
            maxTurnRate = 3f;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (targetTransform)
            {
                target = targetTransform.position;

                // Calculate the combined force from each active behavior
                Vector3 steeringForce = steering.Calulate();

                // Accelaration = Force/Mass
                Vector3 acceleration = steeringForce / rb.mass;
                

                // Update Velocity
                rb.velocity += acceleration * Time.deltaTime;

                transform.Rotate(Velocity.normalized);
                // Do not exeed max velocity
                //Velocity = Velocity.normalized * Mathf.Clamp(Velocity.magnitude, Velocity.magnitude, maxSpeed);

                // Update the position 
                //position += velocity * Time.deltaTime;

                // Update the heading
                //if (Velocity.sqrMagnitude > 0.000001f)
                //{
                //    heading = Velocity.normalized;
                //    //sideComponent = heading.perpendicular;
                //}
            }
            //transform.position = Position;
            //transform.LookAt(heading);
            //transform.rotation = Quaternion.Euler(heading);
        }

        public void Seek(Transform target)
        {
            targetTransform = target;
            steering.seek = true;
        }

        public void Arrive(Transform target)
        {
            steering.arrive = true;
            targetTransform = target;
            
        }
    }
}