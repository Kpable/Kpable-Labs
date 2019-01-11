﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Entity { Soft_Eng }

public class FSMExample : MonoBehaviour {

    
    Text screen;
    SoftEngLes Les;
    ScrollRect scrollRect;
    private void Awake()
    {
        screen = GetComponent<Text>();
        scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
    }

    // Use this for initialization
    void Start () {
        Les = new SoftEngLes((int)Entity.Soft_Eng);
        Les.OutText += WriteToText;

        StartCoroutine("DayOfWork");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WriteToText(int entityId, string text)
    {
        screen.text += "\n" + ((Entity)entityId).ToString() + ": " + text;
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    IEnumerator DayOfWork()
    {
        Les.Update();
        yield return new WaitForSeconds(1);
        StartCoroutine("DayOfWork");
    }
}
