using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.Steering {
    public class AgentController : MonoBehaviour {

        public Transform targets;
        public GameObject agentPrefab;
        public float radius = 20;
        // Use this for initialization
        void Start() {
            for (int i = 0; i < targets.childCount; i++)
            {
                GameObject go = Instantiate(agentPrefab, new Vector3(Random.Range(-radius, radius), 1, Random.Range(-radius, radius)), Quaternion.identity);
                var v = go.GetComponent<Vehicle>();
                v.targetTransform = targets.GetChild(i);
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}