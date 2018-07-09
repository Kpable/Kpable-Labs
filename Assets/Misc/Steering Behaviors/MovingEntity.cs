using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingEntity : BaseGameEntity
    {
        protected Rigidbody rb;
        //protected Vector3 position;
        //protected Vector3 velocity;
        protected Vector3 heading;
        protected Vector3 sideComponent;
        [SerializeField]
        protected float maxSpeed;
        [SerializeField]
        protected float maxForce;
        protected float maxTurnRate;

        //public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        //public Vector3 Velocity { get { return velocity; } set { velocity = value; } }
        public Vector3 Velocity { get { return rb.velocity; } set { rb.velocity = value; } }
        public Vector3 Heading { get { return heading; } set { heading = value; } }

        public float Speed { get { return Velocity.magnitude; } }
        public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        public float MaxForce { get { return maxForce; } set { maxForce = value; } }
        public float MaxTurnRate { get { return maxTurnRate; } set { maxTurnRate = value; } }


        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}