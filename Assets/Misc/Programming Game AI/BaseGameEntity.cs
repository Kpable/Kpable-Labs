using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.AI.FSM
{
    public class BaseGameEntity
    {
        int id;
        public int ID { get { return id; } }

        public BaseGameEntity(int id)
        {
            SetId(id);
        }

        void SetId(int id) { this.id = id; }

        public virtual bool HandleMessage(Telegram message) { return false; }
    }
}