using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Utilities;
using Kpable.AI.FSM;

public class MessageDispatcher : Singleton<MessageDispatcher>
{
    private SortedDictionary<float, Telegram> PriorityQ;

    void Discharge(BaseGameEntity entity, Telegram msg)
    {

    }

    public void DispatchMessage(int sender, int receiver, int msg, float delay = 0f, object extraInfo = null)
    {
        var senderEntity = EntityManager.Instance.GetEntityFromId(sender);
        var receiverEntity = EntityManager.Instance.GetEntityFromId(receiver);

        if (receiverEntity == null)
        {
            Debug.LogWarning("No receiver with id " + receiver);
            return;
        }

        var telegram = new Telegram(sender, receiver, msg, delay, extraInfo);

        if(delay <= 0)
        {
            Discharge(receiverEntity, telegram);
        }
        else
        {
            float currentTime = Time.time;
            telegram.DispatchTime = currentTime + delay;

            PriorityQ.Add(telegram.DispatchTime, telegram);
        }
    }

    public void DispatchDelayedMessages()
    {

    }

}
