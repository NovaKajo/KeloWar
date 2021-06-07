using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kelo.AI{

    public abstract class AIAnimator : MonoBehaviour
    {        
        protected Animator myAnim;
        public bool bIsInitialized = false;

        abstract public void moveForward();   
        abstract public void AttackAnimationAI();
        abstract public void Die(bool value);
        abstract public void GetHit(float position);

        public void Init()
        {
            FindAnimator();
            bIsInitialized = true;
        } 
        
        private void FindAnimator()
        {
            if (myAnim == null)
            {
                if (TryGetComponent(out Animator anim))
                {
                    myAnim = anim;
                    return;
                }
                else if (GetComponentInChildren<Animator>())
                {
                    myAnim = GetComponentInChildren<Animator>();
                    return;
                }
                else
                {
                    Debug.Log("No animator found, please select it on the inspector");
                }
            }
        }
    }
}