using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering {
    public class AgentController : MonoBehaviour {

        public Transform targets;
        public Transform wall;
        public Transform evaders;
        public GameObject agentPrefab;
        public GameObject targetingAgent, popper;
        public float radius = 12;
        public GameObject Mouse;

        // Use this for initialization
        void Start() {
            for (int i = 0; i < targets.childCount; i++)
            {
                GameObject go = Instantiate(agentPrefab, new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), Quaternion.identity);
                var v = go.GetComponent<Vehicle>();
                //v.targetTransform = targets.GetChild(i);
                v.Behaviors.Add(new Behavior(BehaviorType.Arrive, targets.GetChild(i)));
                v.Behaviors.Add(new Behavior(BehaviorType.Flee, Mouse.transform));
                v.Behaviors.Add(new Behavior(BehaviorType.Flee, targetingAgent.transform, 1.15f));

                for (int j = 0; j < wall.childCount; j++)
                {
                    v.Behaviors.Add(new Behavior(BehaviorType.Flee, wall.GetChild(j), 1.1f));
                }
            }

            var targetingAgentVehicle = targetingAgent.GetComponent<Vehicle>();
            targetingAgentVehicle.Behaviors.Add(new Behavior(BehaviorType.Seek, popper.transform));

            for (int k = 0; k < evaders.childCount; k++)
            {
                var vehicle = evaders.GetChild(k).GetComponent<Vehicle>();
                vehicle.Behaviors.Add(new Behavior(BehaviorType.Evade, targetingAgent.GetComponent<Vehicle>()));
                for (int j = 0; j < wall.childCount; j++)
                {
                    vehicle.Behaviors.Add(new Behavior(BehaviorType.Flee, wall.GetChild(j), 1.1f));
                }
            }


        }

        // Update is called once per frame
        void Update() {
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hit))
            //{
            //    Debug.Log(hit.point);

            //    // Do something with the object that was hit by the raycast.
            //}
        }
    }
}