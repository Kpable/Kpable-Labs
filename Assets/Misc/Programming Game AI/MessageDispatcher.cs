using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.Utilities;
using Kpable.AI;
using Kpable.AI.FSM;
using System.Linq;
public class MessageDispatcher : Singleton<MessageDispatcher>
{
    private SortedDictionary<float, Telegram> PriorityQ = new SortedDictionary<float, Telegram>();

    void Discharge(BaseGameEntity entity, Telegram msg)
    {
        if (!entity.HandleMessage(msg))
            Debug.Log("Message not handled");
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
            Debug.Log("Instant Telegram dispatched at time " + Time.time +
                " by " + (Entity)senderEntity.ID + " for " + (Entity)receiverEntity.ID +
                " Msg is " + (MessageType)msg);
        }
        else
        {
            float currentTime = Time.time;
            telegram.DispatchTime = currentTime + delay;

            PriorityQ.Add(telegram.DispatchTime, telegram);

            Debug.Log("Delayed Telegram recorded at time " + Time.time +
                " by " + (Entity)senderEntity.ID + " for " + (Entity)receiverEntity.ID +
                " Msg is " + (MessageType)msg);

        }
    }


    // Called Every update loop
    public void DispatchDelayedMessages()
    {
        float currentTime = Time.time;

        while (PriorityQ.Count > 0 && PriorityQ.First().Value.DispatchTime < currentTime && PriorityQ.First().Value.DispatchTime > 0)
        {
            var telegram = PriorityQ.First().Value;

            BaseGameEntity receiverEntity = EntityManager.Instance.GetEntityFromId(telegram.Receiver);

            Discharge(receiverEntity, telegram);
            Debug.Log("Queued Telegram sent to" + 
                (Entity)receiverEntity.ID + " Msg is " + (MessageType)telegram.MessageType);

            PriorityQ.Remove(telegram.DispatchTime);
        }
    }

}
