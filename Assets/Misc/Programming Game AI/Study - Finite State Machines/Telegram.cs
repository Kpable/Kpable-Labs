using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Telegram
{
    int Sender;
    int Receiver;

    int MessageType;
    public float DispatchTime;
    object ExtraInfo;

    public Telegram(int sender, int receiver, int messageType, float dispatchTime, object extraInfo)
    {
        Sender = sender;
        Receiver = receiver;
        MessageType = messageType;
        DispatchTime = dispatchTime;
        ExtraInfo = extraInfo;
    }
}
