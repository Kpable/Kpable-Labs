﻿//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering
{   
    [System.Serializable]
    public class SteeringBehavior
    {
        // Behavior settings
        public bool seek;
        public bool flee;
        public bool arrive;
        public bool pursuit;
        public bool evade;
        public bool wander;
        public float decelerationModifier = 0.2f;
        public float arrivalRadius = 10f; 


        Vehicle vehicle;

        enum Deceleration { None, Fast, Normal, Slow }


        public SteeringBehavior (Vehicle agent)
        {
            vehicle = agent;
        }

        internal Vector3 Calulate()
        {
            Vector3 totalForce = Vector3.zero;

            if(seek) totalForce += Seek(vehicle.target);
            if(arrive) totalForce += Arrive(vehicle.target, Deceleration.Slow);
            if(flee) totalForce += Flee(vehicle.target);
            if(pursuit) totalForce += Pursuit(vehicle.targetVehicle);
            if(evade) totalForce += Evade(vehicle.targetVehicle);
            if(wander) totalForce += Wander();


            return totalForce;
        }

        /// <summary>
        /// Seek 
        /// 
        /// Return steering fore directing agent towards a target.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        Vector3 Seek (Vector3 targetPosition)
        {
            Vector3 distanceToTarget = targetPosition - vehicle.Position;
            Vector3 desiredVelocity = distanceToTarget.normalized * vehicle.MaxSpeed;
            Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
            steeringForce = steeringForce.normalized * vehicle.MaxForce;
            //Debug.DrawLine(vehicle.Position, desiredVelocity, Color.green);

            return steeringForce;
        }

        Vector3 Flee(Vector3 targetPosition)
        {
            Vector3 distanceToTarget = vehicle.Position - targetPosition;
            Vector3 desiredVelocity = distanceToTarget.normalized * vehicle.MaxSpeed;
            Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
            steeringForce = steeringForce.normalized * vehicle.MaxForce;
            //Debug.DrawLine(vehicle.Position, desiredVelocity, Color.green);

            return steeringForce;
        }

        Vector3 Arrive(Vector3 targetPosition, Deceleration deceleration)
        {
            Vector3 distanceToTarget = targetPosition - vehicle.Position;
            
            float distance = distanceToTarget.magnitude;

            //Debug.Log("distance " + distance);
            //Debug.Log("rad " + arrivalRadius);

            if (distance < arrivalRadius)
            {
                //float speed = distance / ((float)deceleration * decelerationModifier);
                float speed = Map(distance, 0, arrivalRadius, 0, vehicle.MaxSpeed);
                //Debug.Log("deceleration " + ((int)deceleration * decelerationModifier));

                Debug.Log("speed " + speed);
                Vector3 desiredVelocity = distanceToTarget.normalized * speed;
                Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
                steeringForce = steeringForce.normalized * vehicle.MaxForce;
                //speed = Mathf.Clamp(speed, speed, vehicle.MaxSpeed);

                //Vector3 desiredVelocity = toTarget * speed / distance;

                //Debug.Log("speed " + speed + " desiredVelocity " + desiredVelocity);

                return steeringForce;
            }
            else
            {
                Vector3 desiredVelocity = distanceToTarget.normalized * vehicle.MaxSpeed;
                Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
                steeringForce = steeringForce.normalized * vehicle.MaxForce;
                return steeringForce;
            }
        }

        public float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / ( (toMin - fromMin) * (toMax - fromMax) + fromMax ) ;
        }

        Vector3 Pursuit(Vehicle evader)
        {
            Vector3 toEvader = evader.Position - vehicle.Position;

            float relativeHeading = Vector3.Dot(vehicle.Heading, evader.Heading);

            if(Vector3.Dot(toEvader, vehicle.Heading) > 0 && relativeHeading < -0.95f)
            {
                return Seek(evader.Position);
            }

            float lookAheadTime = toEvader.magnitude / (vehicle.MaxSpeed * evader.Speed);

            return Seek(evader.Position + evader.Velocity * lookAheadTime);
        }

        Vector3 Evade(Vehicle pursuer)
        {
            Vector3 toPursuer = pursuer.Position - vehicle.Position;

            float threatRange = 100f;
            if (toPursuer.sqrMagnitude > threatRange * threatRange) return Vector3.zero;

            float lookAheadTime = toPursuer.magnitude / (vehicle.MaxSpeed * pursuer.Speed);

            return Flee(pursuer.Position + pursuer.Velocity * lookAheadTime);
        }

        Vector3 Wander()
        {
            Vector3 wanderTarget = Vector3.zero;
            float wanderRadius = 10f;
            float wanderDistance = 5f;

            wanderTarget += new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));

            wanderTarget.Normalize();

            wanderTarget *= wanderRadius;

            Vector3 target = wanderTarget + new Vector3(wanderDistance, 0, 0);

            return target - vehicle.Position;
        }

        Vector3 ObstacleAvoidance( GameObject[] obstacles)
        {
            //// Define the detection length of the box proportional to the agent's velocity
            //float minBoxlength = 40f;
            //float boxLength = minBoxlength + (vehicle.Speed / vehicle.MaxSpeed) * minBoxlength;

            // grab a list of obstacles in that path
            //Physics.OverlapBox()
            
            return Vector3.zero;
        }
    }
}