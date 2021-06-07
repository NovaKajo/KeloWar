using System;
using System.Collections;
using System.Collections.Generic;
using Kelo.Combat;
using Kelo.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Kelo.Stats
{

    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent Death;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        private int currentHealth;
        public Action<float> OnHealthPctChanged = delegate { };
        bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;

        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int damagevalue)
        {
            currentHealth = currentHealth - damagevalue;
            if (currentHealth <= 0 && !isDead)
            {
                //myAnim.ResetTrigger("resurrect");
                Die();
            }
            else
            {
                ModifyHealth(damagevalue);

            }
        }
        
        private void Die()
        {
            //Death.Invoke();
            isDead = true;
            if(this.gameObject.CompareTag("Enemy"))
            {
               this.gameObject.GetComponent<Enemy>().RemoveFromList();
            }
            //myAnim.SetTrigger("isAlive");

            GetComponent<Scheduler>().CancelCurrentAction();
            
            gameObject.SetActive(false);
        }
        
        public void ModifyHealth(int amount)
        {
           
            float currentHealthPct = (float)currentHealth / (float)maxHealth;
            OnHealthPctChanged(currentHealthPct);
        }
    }
}