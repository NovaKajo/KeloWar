using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kelo.Stats
{

    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;

        private int currentHealth;
        public Action<float> OnHealthPctChanged = delegate { };
        bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;

        }

        private void ChangeHealth()
        {

            ModifyHealth(-5);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int damagevalue)
        {
            ModifyHealth(damagevalue);
        }
        
        public void ModifyHealth(int amount)
        {
            currentHealth += amount;
            float currentHealthPct = (float)currentHealth / (float)maxHealth;
            OnHealthPctChanged(currentHealthPct);
        }
    }
}