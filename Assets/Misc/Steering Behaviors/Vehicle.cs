using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering
{
    public class Vehicle : MovingEntity
    {
        SteeringBehavior steering;

        public Vector3 target;
        public Transform targetTransform;
        public Vehicle targetVehicle;
        [SerializeField]
        Behavior[] behaviors;

        public List<Behavior> Behaviors { get { return steering.behaviors; } }

        protected override void Awake()
        {
            base.Awake();
            steering = new SteeringBehavior(this);


        }
        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < behaviors.Length; i++)
            {
                Behaviors.Add(behaviors[i]);
            }

            //Position = transform.position;
            //Velocity = Vector3.zero;
            //heading = Velocity;
            //maxSpeed = 15f;
            //maxForce = 2f;
            //maxTurnRate = 3f;

            //Testing Map function
            //Debug.Log(steering.Map(2, 1, 3, 0, 10)); // 5
            //Debug.Log(steering.Map(5, 0, 10, 1, 3)); // 2
            //Debug.Log(steering.Map(20, 0, 40, 1, 5)); // 3


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            //target = targetTransform.position;

            // Calculate the combined force from each active behavior
            Vector3 steeringForce = steering.Calulate();

            // Accelaration = Force/Mass
            Vector3 acceleration = steeringForce / rb.mass;

            acceleration.y = 0;

            // Update Velocity
            rb.velocity += acceleration;

            //transform.Rotate(Velocity.normalized);
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
            
            //transform.position = Position;
            //transform.LookAt(heading);
            //transform.rotation = Quaternion.Euler(heading);
        }

        public void Seek()
        {
            //targetTransform = target;
            //steering.seek = true;
            steering.behaviors.Add(new Behavior(BehaviorType.Arrive, targetTransform));
            steering.behaviors.Add(new Behavior(BehaviorType.Flee, targetTransform));

        }

        public void Arrive(Transform target)
        {
            targetTransform = target;
            steering.arrive = true;
        }

        public void Flee(Transform target)
        {
            targetTransform = target;
            steering.flee = true;
        }
    }
}