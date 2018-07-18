﻿//using System;
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
        Wander, 
        ObstacleAvoidance
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
        public Vector3 wanderTarget;
        public float wanderRadius = 5f;
        public float wanderDistance = 8f;
        public float wanderJitter = 2f;
        public float obstacleAvoidanceDetectionBoxLength = 10f;
        public float brakingWeight = 0.2f;

        //Debug Param
        public Vector3 wanderRand;
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
                case BehaviorType.ObstacleAvoidance:
                    steeringForce = ObstacleAvoidance();
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
            

            Vector3 randomVector = new Vector3(Random.Range(-1f, 1f) * wanderJitter,0, Random.Range(-1f, 1f));
            wanderTarget += randomVector;

            wanderTarget.Normalize();

            //Debug.DrawLine(vehicle.Position, wanderTarget, Color.blue);

            wanderTarget *= wanderRadius;
            wanderRand = wanderTarget;

            //Debug.DrawLine(vehicle.Position, wanderTarget, Color.green);

            Vector3 target = wanderTarget + new Vector3(wanderDistance, 0, 0);
            //Debug.Log("wander target: " + target);
            target += vehicle.Position;
            //Debug.Log("tranform space: " + vehicle.transform.InverseTransformVector(target));
            target = vehicle.transform.InverseTransformVector(target);
            //Debug.DrawLine(vehicle.Position, target, Color.red);
            //Debug.DrawLine(vehicle.Position, vehicle.transform.InverseTransformVector(target), Color.magenta);

            Vector3 steeringForce = target - vehicle.Position;
            steeringForce = steeringForce.normalized * Mathf.Clamp(steeringForce.magnitude, 0, vehicle.MaxForce);

            //Debug.DrawLine(vehicle.Position, vehicle.Position + steeringForce, Color.black);

            //Debug.Log("steering force: " + steeringForce);

            return steeringForce;
        }

        Vector3 ObstacleAvoidance()
        {
            //// Define the detection length of the box proportional to the agent's velocity
            //float minBoxlength = 40f;
            float detectionBoxLength = obstacleAvoidanceDetectionBoxLength + (vehicle.Speed / vehicle.MaxSpeed) * obstacleAvoidanceDetectionBoxLength;
            Vector3 localPosOfClosestObstacle = Vector3.zero;
            float distanceToClosest = float.MaxValue;
            GameObject closestObject = null;

            // grab a list of obstacles in that path
            Collider[] objectsDetected = Physics.OverlapBox(vehicle.Position + new Vector3(detectionBoxLength / 2,0), new Vector3(detectionBoxLength / 2, .5f, .5f));
            Debug.Log("Obstacles Detected: " + objectsDetected.Length);

            for (int i = 0; i < objectsDetected.Length; i++)
            {
                Debug.Log(objectsDetected[i].name + ": " + objectsDetected[i].transform.position);

                float distance = Vector3.Distance(vehicle.Position, objectsDetected[i].transform.position);
                if(distance < distanceToClosest)
                {
                    distanceToClosest = distance;
                    localPosOfClosestObstacle = objectsDetected[i].transform.position;
                    closestObject = objectsDetected[i].gameObject;
                }
            }
            
            Debug.DrawLine(vehicle.Position, new Vector3(vehicle.Position.x + detectionBoxLength, vehicle.Position.y, vehicle.Position.z));

            Vector3 steeringForce = Vector3.zero;

            if(closestObject)
            {
                float multiplier = 1f + (detectionBoxLength - localPosOfClosestObstacle.x) / detectionBoxLength;
                Debug.Log("Multiplier: " + multiplier);
                steeringForce.z = (closestObject.GetComponent<Collider>().bounds.size.z - localPosOfClosestObstacle.y) * multiplier;

                steeringForce.x = (closestObject.GetComponent<Collider>().bounds.size.x - localPosOfClosestObstacle.x) *brakingWeight;

            }

            return steeringForce;
        }
    }
}