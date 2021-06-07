using Kelo.Core;
using Kelo.Enemies;
using Kelo.Stats;
using UnityEngine;

namespace Kelo.Player
{

    public class PlayerAttack : MonoBehaviour,IAction
{
    [Header("TargetVFX")]
    public Transform targetVFX;

    [Header("Weapons")]
    [SerializeField] private Transform SwordGJ;
    [SerializeField] private Transform BowGJ;
    [SerializeField] public Transform arrowInHand;

    private Health playerHealth;

    [SerializeField] private float swordRange = 3f;
    [SerializeField] private float lookAtSpeed = 10f;
    
    [Range(0.2f, 3f)]
    [SerializeField] private float attackTime = 0.5f;
    [Range(1.5f,3f)]
    [SerializeField] private float bowAttackSpeed = 2f;
  

    private float lastTimesinceAttack = Mathf.Infinity;

    private Animator animator;

    private int attackHash = Animator.StringToHash("Attack1");
    private int bowHash = Animator.StringToHash("Bow");

    Scheduler scheduler;

    private Enemy targetEnemy;
    
    private Health targetHealth;

    public bool canAttack = false;
    
    private void Start() {
        scheduler = GetComponent<Scheduler>();
        animator = GetComponentInChildren<Animator>();
        playerHealth = GetComponent<Health>();
   
    }

    private void Update() {
        animator.SetFloat("bowAttackSpeed",bowAttackSpeed); // evitable solo por motivos de pruebas
        canAttack = false;
        lastTimesinceAttack += Time.deltaTime;
        if(!playerHealth.IsDead())
        {

            LookAtTarget();
            if(lastTimesinceAttack>attackTime)
            {
               
            AttackTarget();

            }
        }
    }

    public void LookAtTarget()
    {
        
        if (targetEnemy == null || targetHealth.IsDead())
        {
            return;
        }

        // Smoothly rotate towards the target point.
        var targetRotation = Quaternion.LookRotation(targetEnemy.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
       
    }

    private void AttackTarget()
    {
        

        if (targetEnemy == null || targetHealth.IsDead())
        {        
            if(targetHealth != null)
            {
                    EnemyList.FindClosestEnemy(this.transform);
                    SetTarget(EnemyList.closestEnemyToPlayer);
            }
            return;
        }
        if(!canAttack)
        {
            return;
        }
        lastTimesinceAttack = 0;                
        if (Vector3.Distance(targetEnemy.transform.position, this.transform.position) < swordRange)
        {
            HandleSwordAttack();
        }
        else
        {
            //Debug.Log("attack with bow");
            HandleBowAttack();
        }
    }

    private void HandleBowAttack()
    {
        SwordGJ.gameObject.SetActive(false);
        arrowInHand.gameObject.SetActive(true);
        BowGJ.gameObject.SetActive(true);
        if(!animator.GetCurrentAnimatorStateInfo(2).IsName("Attack01_Bow")){

        animator.SetTrigger(bowHash);
        }
    }

    private void HandleSwordAttack()
    {
        BowGJ.gameObject.SetActive(false);
        arrowInHand.gameObject.SetActive(false);
        SwordGJ.gameObject.SetActive(true);
        animator.SetTrigger(attackHash);
    }

    public void DamageTarget()
    {
        if(targetEnemy == null)
        {
            Debug.Log("no target");
            return;
        }
      targetEnemy.GetComponent<Health>().TakeDamage(30);
    }

    public void Disengage()
    {
        targetVFX.gameObject.SetActive(false);
        targetEnemy = null;
    }

    public void SetTarget(Enemy targetToHit)
    {
        if(targetToHit != null)
        {

        targetEnemy = targetToHit;
        targetHealth = targetToHit.GetComponent<Health>();    
        if(targetHealth.IsDead())   
        return;
        targetVFX.transform.position = targetEnemy.transform.position+Vector3.up/8;
        targetVFX.transform.parent = targetEnemy.transform;
        targetVFX.gameObject.SetActive(true);
        }
    }

    public Enemy GetTarget()
    {
      
        return targetEnemy;
    }
}

}