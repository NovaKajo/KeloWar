using System;
using Kelo.AI;
using Kelo.Core;
using Kelo.Enemies;
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

        private void Awake()
        {
            currentHealth = maxHealth;

        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        public int GetMaxHealth()
        {
            return maxHealth;
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
                if (GetComponent<AIAnimator>())
                {
                    GetComponent<AIAnimator>().GetHit(0); // debe haber alguna funcion matematica para ver de donde viene el golpe
                }

            }
        }
        
        private void Die()
        {
            //Death.Invoke();
            isDead = true;
            if(this.gameObject.CompareTag("Enemy"))
            {
                this.gameObject.GetComponent<Enemy>().RemoveFromList();
                this.gameObject.GetComponent<AIFighter>().Disengage();
                DeathAnimationAI();
            }

            GetComponent<Scheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void DeathAnimationAI()
        {
            if (GetComponent<AIAnimator>())
            {
                GetComponent<AIAnimator>().Die(false);
            }
            else
            {
                Debug.LogWarning("No animation death behaviour");
            }
        }

        public void ModifyHealth(int amount)
        {
           
            float currentHealthPct = (float)currentHealth / (float)maxHealth;
            OnHealthPctChanged(currentHealthPct);
        }
    }
}