//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering
{   public enum BehaviorType
    {
        Seek,
        Flee,
        Arrive,
        Pursuit,
        Evade,
        Wander
    }

    [System.Serializable]
    public class Behavior
    {
        public BehaviorType behaviorType;
        public Transform target;
        public Vehicle targetVehicle;
        public float weight = 1;

        public Behavior(BehaviorType behaviorType, Transform target)
        {
            this.behaviorType = behaviorType;
            this.target = target;
        }

        public Behavior(BehaviorType behaviorType, Vehicle target)
        {
            this.behaviorType = behaviorType;
            this.targetVehicle = target;
        }

        public Behavior(BehaviorType behaviorType, Transform target, float weight)
        {
            this.behaviorType = behaviorType;
            this.target = target;
            this.weight = weight;
        }

    }

    [System.Serializable]
    public class SteeringBehavior
    {
        public List<Behavior> behaviors = new List<Behavior>();
        // Behavior settings
        public bool seek;
        public bool flee;
        public bool arrive;
        public bool pursuit;
        public bool evade;
        public bool wander;
        public float deceleration = 0.2f;
        public float arrivalSlowRadius = 10f;
        public float arrivalStopRadius = 0.001f;
        public float fleeRadius = 5f;
        public float evadeThreatRange = 10f;
        float wanderRadius = 1.5f;
        float wanderDistance = 2f;

        Vehicle vehicle;

        enum Deceleration { None, Fast, Normal, Slow }


        public SteeringBehavior (Vehicle agent)
        {
            vehicle = agent;
        }

        internal Vector3 Calulate()
        {
            Vector3 totalForce = Vector3.zero;

            //if(seek) totalForce += Seek(vehicle.target);
            ////if(arrive) totalForce += Arrive(vehicle.target, Deceleration.Slow);
            //if(flee) totalForce += Flee(vehicle.target);
            //if(pursuit) totalForce += Pursuit(vehicle.targetVehicle);
            //if(evade) totalForce += Evade(vehicle.targetVehicle);
            //if(wander) totalForce += Wander();

            foreach(var b in behaviors)
            {
                totalForce += ProcessBehavior(b);
            }

            return totalForce;
        }

        Vector3 ProcessBehavior(Behavior behavior)
        {
            Vector3 steeringForce = Vector3.zero;

            switch (behavior.behaviorType)
            {
                case BehaviorType.Seek:
                    steeringForce = Seek(behavior.target.position);
                    break;
                case BehaviorType.Flee:
                    steeringForce = Flee(behavior.target.position);
                    break;
                case BehaviorType.Arrive:
                    steeringForce = Arrive(behavior.target.position);
                    break;
                case BehaviorType.Pursuit:
                    //steeringForce = Pursuit(behavior.target.position);
                    break;
                case BehaviorType.Evade:
                    steeringForce = Evade(behavior.targetVehicle);
                    break;
                case BehaviorType.Wander:
                    steeringForce = Wander();
                    break;
                default:
                    break;
            }

            steeringForce *= behavior.weight;

            return steeringForce;
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
            steeringForce = steeringForce.normalized * Mathf.Clamp(steeringForce.magnitude, 0, vehicle.MaxForce);
            //Debug.DrawLine(vehicle.Position, desiredVelocity, Color.green);

            return steeringForce;
        }

        Vector3 Flee(Vector3 targetPosition)
        {
            // DEBUG Track Mouse
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hit))
            //{
            //    targetPosition.x = hit.point.x;
            //    targetPosition.z = hit.point.z;
            //}
            // END DEBUG
            Vector3 distanceToTarget = vehicle.Position - targetPosition;
            float distance = distanceToTarget.magnitude;
            // If we are farther than the radius, dont flee
            if (distance > fleeRadius)
            {
                return Vector3.zero;
            }
            // else, run
            Vector3 desiredVelocity = distanceToTarget.normalized * vehicle.MaxSpeed;
            Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
            steeringForce = steeringForce.normalized * Mathf.Clamp(steeringForce.magnitude, 0, vehicle.MaxForce);
            //Debug.DrawLine(vehicle.Position, desiredVelocity, Color.green);

            return steeringForce;
        }

        Vector3 Arrive(Vector3 targetPosition)
        {
            Vector3 distanceToTarget = targetPosition - vehicle.Position;
            
            float distance = distanceToTarget.magnitude;

            //Debug.Log("distance " + distance);
            //Debug.Log("rad " + arrivalRadius);
            Vector3 desiredVelocity = Vector3.zero;
            if(distance < arrivalStopRadius)
            {
               return desiredVelocity;
            }
            else if (distance < arrivalSlowRadius)
            {
                //float speed = distance / ((float)deceleration * decelerationModifier);
                float speed = Map(distance, 0, arrivalSlowRadius, 0, vehicle.MaxSpeed);
                //Debug.Log("deceleration " + ((int)deceleration * decelerationModifier));
                
                //Debug.Log("speed " + speed + " distance " + distance);
                desiredVelocity = distanceToTarget.normalized * speed;
                //speed = Mathf.Clamp(speed, speed, vehicle.MaxSpeed);

                //Vector3 desiredVelocity = toTarget * speed / distance;

                //Debug.Log("speed " + speed + " desiredVelocity " + desiredVelocity);

            }
            else
            {
                desiredVelocity = distanceToTarget.normalized * vehicle.MaxSpeed;
            }

            Vector3 steeringForce = desiredVelocity - vehicle.Velocity;
            steeringForce = steeringForce.normalized * Mathf.Clamp(steeringForce.magnitude, 0, vehicle.MaxForce);
            //Debug.Log("Arrival SteeringForce: " + steeringForce.magnitude );
            return steeringForce;
        }

        public float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) /  (fromMax - fromMin) * (toMax - toMin) + toMin;
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

            if(toPursuer.sqrMagnitude > evadeThreatRange * evadeThreatRange)
                return Vector3.zero;


            float lookAheadTime = toPursuer.magnitude / (vehicle.MaxSpeed * pursuer.Speed);

            return Flee(pursuer.Position + pursuer.Velocity * lookAheadTime);
        }

        Vector3 Wander()
        {
            Vector3 wanderTarget = Vector3.zero;

            Vector3 randomVector = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            wanderTarget += randomVector;

            wanderTarget.Normalize();

            wanderTarget *= wanderRadius;

            Vector3 target = wanderTarget + new Vector3(wanderDistance, 0, 0);
            Debug.Log("wander target: " + target);

            Vector3 steeringForce = target - vehicle.Position;
            steeringForce = steeringForce.normalized * Mathf.Clamp(steeringForce.magnitude, 0, vehicle.MaxForce);

            return steeringForce;
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