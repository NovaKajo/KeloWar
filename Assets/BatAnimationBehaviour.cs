using System.Collections;
using System.Collections.Generic;
using Kelo.AI;
using UnityEngine;

public class BatAnimationBehaviour : AIAnimator
{
    private int attackHash = Animator.StringToHash("Attack");
    private int isAliveHash = Animator.StringToHash("isAlive");

   private void Awake() {
        Init();
   }

    public override void AttackAnimationAI()
    {
        myAnim.SetTrigger(attackHash);
    }

    public override void Die(bool value)
    {
        myAnim.SetBool(isAliveHash, value);
    }

    public override void GetHit(float position)
    {
        switch (position)
        {
            case 0:
                myAnim.SetTrigger("GetHitFront");
                break;
            case 1:
                myAnim.SetTrigger("GetHitRight");
                break;
            case 2:
                myAnim.SetTrigger("GetHitLeft");
                break;
            default:
                myAnim.SetTrigger("GetHitFront");
                break;
        }
    }

    public override void moveForward()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
