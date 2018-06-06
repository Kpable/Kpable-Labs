﻿using UnityEngine;
using System.Collections;

namespace Kpable.Tutorials.AlienRunner
{
    public class BGScaler : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var height = Camera.main.orthographicSize * 2f;
            var width = height * Screen.width / Screen.height;

            if (gameObject.name == "Background")
            {
                transform.localScale = new Vector3(width, height, 0);
            }
            else
            {
                transform.localScale = new Vector3(width + 5f, 5, -1);
            }
        }

    }
}
