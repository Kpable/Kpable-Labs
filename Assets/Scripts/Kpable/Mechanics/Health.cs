using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kpable.Mechanics
{
    public class Health : MonoBehaviour
    {

        [SerializeField]
        private int currentHealth;
        [SerializeField]
        private int maxHealth;

        public int CurrentHealth { get { return currentHealth; } set { currentHealth = Mathf.Clamp(value, 0, maxHealth); } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = Mathf.Clamp(value, 0, value); } }

        public delegate void ValueChange();
        public ValueChange OnValueChanged;

        public delegate void HealthBelowMax();
        public HealthReachedZero OnHealthDroppedBelowMax;
        public delegate void HealthBelow75Percent();
        public HealthBelow75Percent OnHealthDroppedBelow75Percent;
        public delegate void HealthBelow50Percent();
        public HealthBelow50Percent OnHealthDroppedBelow50Percent;
        public delegate void HealthBelow25Percent();
        public HealthBelow25Percent OnHealthDroppedBelow25Percento;
        public delegate void HealthReachedZero();
        public HealthReachedZero OnHealthDroppedToZero;

        public void AddHealth(int amount)
        {

        }

        public void Damage(int amount)
        {
            if (currentHealth > 0)
            {
                currentHealth -= amount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

                if (currentHealth == 0)
                {
                    if (OnHealthDroppedToZero != null)
                        OnHealthDroppedToZero();
                }
            }
        }

        public void Start()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
}