using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using Kelo.Stats;
using UnityEngine;

namespace Kelo.AI
{

    public class AIFighter : MonoBehaviour,IAction
{
    AIAnimator myAnim;
    float timeSinceLastAttack = Mathf.Infinity;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] float TimeBetweenAttacks = 1f;
    [SerializeField] int figtherDamage = 5;
    [SerializeField]Transform target;
    [SerializeField] bool canMove = true;

    [SerializeField] bool basicAttackBehaviour = true;


    private AIMover AImover;


    public AttackBehaviour attack;
    //[SerializeField] private Health health;

    private void Awake()
    {
        
        if(GetComponent<AIAnimator>())
        myAnim = GetComponent<AIAnimator>();
        AImover = GetComponent<AIMover>();

    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (target == null) return;
        if (target.GetComponentInParent<Health>().IsDead()) return;
        
        if(canMove)
        {
        AImover.MoveTo(target.position, 1f);
        }

        if (IsInRange(target.transform))
        {

            AImover.Disengage();
            if (timeSinceLastAttack >= TimeBetweenAttacks)
            {
                //Triggers Hit() animation dmg
               // Debug.Log("attacking");
                AttackBehaviour();
                timeSinceLastAttack = 0;
            }
        }
            //codigo de pruebas
            Vector3 VectorResult;
            float DotResult = Vector3.Dot(transform.forward, target.forward);
            if (DotResult > 0)
            {
                VectorResult = transform.forward + target.forward;
            }
            else
            {
                VectorResult = transform.forward - target.forward;
            }
            Debug.DrawRay(transform.position, VectorResult * 100, Color.green);
    }

    private bool IsInRange(Transform targetTransform)
    {
        return Vector3.Distance(transform.position, targetTransform.position) < attackRange;
    }

    public bool CanAttack(GameObject combatTarget)
    {       
        if (combatTarget == null) { return false; }
        if (!GetComponent<AIMover>().CanMoveTo(combatTarget.transform.position)
        && !IsInRange(combatTarget.transform))
        {          
            return false;
        }
     
        Health targetToTest = combatTarget.GetComponentInParent<Health>(); // deberia cambiarlo?
        return targetToTest != null && !targetToTest.IsDead();
    }

    private void AttackBehaviour()
    {
        transform.LookAt(target);
        if(basicAttackBehaviour)
        {
        target.GetComponent<Health>().TakeDamage(figtherDamage); // deberia ser con evento de animacion pero placeholders
        TriggerAttack(); //attack animation
        }else{
            attack.Execute(target,transform,transform.localRotation);
        }
    }

    private void TriggerAttack()
    {
        if(myAnim == null)
        {
            Debug.LogWarning("No attack animation behaviour"+gameObject.name);
            return;
        }
        if(!this.GetComponent<Health>().IsDead())
        {            
            myAnim.AttackAnimationAI();
        }
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
        TriggerStopAttacking();
        target = null;
    }

    private void TriggerStopAttacking()
    {
        myAnim.ResetTrigger("Attack");
    }


    }

}
