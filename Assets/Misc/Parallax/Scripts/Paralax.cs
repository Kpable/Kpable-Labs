using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.Mechanics
{ 

    public class Paralax : MonoBehaviour
    {
        public GameObject target;
        public GameObject[] backgrounds;

        public float scrollSpeed = -30f;
        public float motionMult = 0.25f;

        private float panelHt;
        private float depth;
        
        // Use this for initialization
        void Start()
        {
            panelHt = backgrounds[0].transform.localScale.y;
            depth = backgrounds[0].transform.localScale.z;

            backgrounds[0].transform.position = new Vector3(0, 0, depth);
            backgrounds[1].transform.position = new Vector3(0, panelHt, depth);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float tY = 0, tX = 0;

            if (target != null)
            {
                tX = -target.transform.position.x * motionMult;
                tY = -target.transform.position.y * motionMult;
            }

            backgrounds[0].transform.position = new Vector3(tX, tY, depth);
            backgrounds[1].transform.position = new Vector3(tX, tY - panelHt, depth);

            //if (tY >= 0)
            //{
            //    backgrounds[1].transform.position = new Vector3(tX, tY - panelHt, depth);
            //}
            //else
            //{
            //    backgrounds[1].transform.position = new Vector3(tX, tY + panelHt, depth);
            //}
        }
    }
}