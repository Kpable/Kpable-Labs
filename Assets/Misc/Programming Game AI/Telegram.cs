using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.FSM
{

    public struct Telegram
    {
        public int Sender;
        public int Receiver;

        public int MessageType;
        public float DispatchTime;
        public object ExtraInfo;

        public Telegram(int sender, int receiver, int messageType, float dispatchTime, object extraInfo)
        {
            Sender = sender;
            Receiver = receiver;
            MessageType = messageType;
            DispatchTime = dispatchTime;
            ExtraInfo = extraInfo;
        }
    }
}
