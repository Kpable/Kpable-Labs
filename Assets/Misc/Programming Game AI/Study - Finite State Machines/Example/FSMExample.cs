using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Kpable.AI.FSM;

public enum Entity { Soft_Eng, Pet_Robot }
public enum MessageType
{
    Msg_EkkoImHome,
    Msg_DevicesPrimed
}

public class FSMExample : MonoBehaviour {

    
    Text screen;
    SoftEngLes Les;
    PetRobotEkko Ekko;
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
        // This registration could potentially be moved to the base game entity. 
        EntityManager.Instance.RegisterEntity(Les);

        Ekko = new PetRobotEkko((int)Entity.Pet_Robot);
        Ekko.OutText += WriteToText;
        EntityManager.Instance.RegisterEntity(Ekko);

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
        Ekko.Update();
        yield return new WaitForSeconds(1);
        StartCoroutine("DayOfWork");
    }
}
