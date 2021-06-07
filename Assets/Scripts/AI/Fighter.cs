using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using Kelo.Stats;
using UnityEngine;

namespace Kelo.AI
{

    public class Fighter : MonoBehaviour,IAction
{
    Animator myAnim;
    float timeSinceLastAttack = Mathf.Infinity;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] float TimeBetweenAttacks = 1f;

    Transform target;

    //[SerializeField] private Health health;

    private void Awake()
    {
        if(GetComponent<Animator>())
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (target == null) return;
        if (target.GetComponentInParent<Health>().IsDead()) return;

        GetComponent<AIMover>().MoveTo(target.position, 1f);
        if (IsInRange(target.transform))
        {

            GetComponent<AIMover>().Disengage();
            if (timeSinceLastAttack >= TimeBetweenAttacks)
            {
                //Triggers Hit() animation dmg
                Debug.Log("attacking");
                AttackBehaviour();
                timeSinceLastAttack = 0;
            }
        }

    }

    private bool IsInRange(Transform targetTransform)
    {

        return Vector3.Distance(transform.position, targetTransform.position) < attackRange;

    }

    public bool CanAttack(GameObject combatTarget)
    {
       
        if (combatTarget == null) { return false; }
        if (!GetComponent<AIMover>().CanMoveTo(combatTarget.transform.position)
        && !IsInRange(combatTarget.transform)
        )
        {
          
            return false;
        }
     
        Health targetToTest = combatTarget.GetComponentInParent<Health>(); // deberia cambiarlo?
        return targetToTest != null && !targetToTest.IsDead();
    }

    private void AttackBehaviour()
    {
        transform.LookAt(target);
        
        //TriggerAttack(); needs animator
    }

    private void TriggerAttack()
    {
        myAnim.ResetTrigger("stopAttacking");
        myAnim.SetTrigger("Attack");
    }

    public void Attack(GameObject combatTarget)
    {
        GetComponent<Scheduler>().StartAction(this);
        //Debug.Log("attacking");
        target = combatTarget.transform;
    }

    public void Disengage()
    {
        if (target == null) return;
        GetComponent<AIMover>().Disengage();
        //TriggerStopAttacking();
        target = null;
    }

}

}