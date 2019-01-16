using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI
{
    public class BaseGameEntityBehaviour : MonoBehaviour
    {
        BaseGameEntity entity;

        public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
        public float BoundingRadius { get; set; }

        private void Awake()
        {
            
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