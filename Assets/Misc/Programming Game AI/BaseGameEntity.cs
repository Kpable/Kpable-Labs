using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kpable.AI.FSM;

namespace Kpable.AI
{
    public class BaseGameEntity
    {
        public int ID { get; private set; }

        public BaseGameEntity(int id)
        {
            SetId(id);
        }

        void SetId(int id) { ID = id; }

        public virtual bool HandleMessage(Telegram message) { return false; }
    }
}