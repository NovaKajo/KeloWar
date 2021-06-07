using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using Kelo.Combat;
using Kelo.Stats;
using UnityEngine;

namespace Kelo.AI
{
    public class AIFighter : MonoBehaviour,IAction
{
    AIAnimator myAnim;

    float timeSinceLastAttack = Mathf.Infinity;
    float timeSinceLastCollisionDamage = Mathf.Infinity;
    [Header("Target and Behaviour")]
    [SerializeField] bool doesDamageOnCollision = true;
    [SerializeField] bool canMove = true;
    [SerializeField] bool alwaysLookAtTarget = false;
    [SerializeField] bool basicAttackBehaviour = true;
    [Space(5)]
    [SerializeField] AttackBehaviour attackBehaviour;
    [Space(10)]
    [Header("Range and time between damage")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] float TimeBetweenAttacks = 1f;
    [SerializeField] float timeBetweenCollisionDamage = 1f;

    [Header("Fighter Base Damage")]
    [SerializeField] int figtherDamage = 5;
    [SerializeField] int collisionDamage = 5;
    private List<string> collisionTag ;
    [SerializeField] Transform target;

    [Header("Reclaimer")]
    [SerializeField] bool isReclaimer = false;
    bool weaponReclaimed = true; 

    private bool isAggravated = false;
    private AIMover AImover;

    
    //[SerializeField] private Health health;

    private void Awake()
    {
        
        if(GetComponent<AIAnimator>())
        myAnim = GetComponent<AIAnimator>();
        AImover = GetComponent<AIMover>();

    }
    
    public void SetWeaponReclaim(bool state)
    {
        weaponReclaimed = state;
    }
    public void SetAggravated(bool state){
        isAggravated = state;
    }
    public void SetCollisionTag(List<string> value)
    {
        collisionTag = value;
    }
    public float getAttackRange()
    {
        return attackRange;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastCollisionDamage += Time.deltaTime;
        
        if (target == null) return;
        if(!isAggravated) return;
        if (target.GetComponentInParent<Health>().IsDead()) return;
        
        if(canMove)
        {
        AImover.MoveTo(target.position, 1f);
        }
        if(alwaysLookAtTarget)
        {
                transform.LookAt(target);
        }

       
        if (IsInRange(target.transform))
        {

            AImover.Disengage();
                if (isReclaimer)
                {

                    if (!weaponReclaimed) return;
                }

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
        if (!GetComponent<AIMover>().CanMoveTo(combatTarget.transform.position) // esto corre en update
        && !IsInRange(combatTarget.transform))
        {          
            return false;
        }
     
        Health targetToTest = combatTarget.GetComponentInParent<Health>(); // deberia cambiarlo?
        return targetToTest != null && !targetToTest.IsDead();
    }

    public bool CanAttack()
    {
        if (target == null) { return false; }
        if (!GetComponent<AIMover>().CanMoveTo(target.transform.position) // esto corre en update
        && !IsInRange(target.transform))
        {
            return false;
        }

        Health targetToTest = target.GetComponentInParent<Health>(); // deberia cambiarlo?
        return targetToTest != null && !targetToTest.IsDead();
    }

    private void AttackBehaviour()
    {
        if(!alwaysLookAtTarget)
        {
            transform.LookAt(target);
        }
        
        if(basicAttackBehaviour)
        {
            target.GetComponent<Health>().TakeDamage(figtherDamage); // deberia ser con evento de animacion pero placeholders
            TriggerAttack(); //attack animation
        }else{
            weaponReclaimed = false;
            attackBehaviour.Execute(target,transform,transform.localRotation);
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

    public void AttackCurrentTarget()
    {
        GetComponent<Scheduler>().StartAction(this);
        
    }
    public void SetTarget(Transform combatTarget)
    {
        target = combatTarget;
    }

    public Transform GetTarget()
    {
        return target;
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

    private void OnCollisionEnter(Collision other)
    {
            bool foundOne = CheckTags(other);
            if (!foundOne) return;
        if(!doesDamageOnCollision) return;

            if (other.gameObject.TryGetComponent(out Health helth))
            {
                helth.TakeDamage(collisionDamage);
            }
            timeSinceLastCollisionDamage = 0;

            
    }
    private void OnCollisionStay(Collision other)
        {
            bool foundOne = CheckTags(other);
            if (!foundOne) return;
            if (!doesDamageOnCollision) return;

            if (timeSinceLastCollisionDamage >= timeBetweenCollisionDamage)
            {
                if (other.gameObject.TryGetComponent(out Health helth))
                {
                    helth.TakeDamage(collisionDamage);
                }
            }
            timeSinceLastCollisionDamage = 0;
        }

        private bool CheckTags(Collision other)
        {
            bool foundOne = false;
            for (int i = 0; i < collisionTag.Count; i++)
            {
                if (other.gameObject.CompareTag(collisionTag[i]))
                {
                    foundOne = true;
                    continue;
                }

            }

            return foundOne;
        }
    }

}
