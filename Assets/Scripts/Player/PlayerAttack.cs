using System;
using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
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

    [SerializeField] private float swordRange = 3f;
    [SerializeField] private float lookAtSpeed = 10f;
    [SerializeField] private float attackTime = 1f;
    private float lastTimesinceAttack = Mathf.Infinity;

    private Animator animator;
    Scheduler scheduler;

    private Enemy targetEnemy;
    
    private Health targetHealth;

    public bool readyToAttack = false;
    
    private void Start() {
        scheduler = GetComponent<Scheduler>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        
        LookAtTarget();
        if(lastTimesinceAttack>=attackTime)
        {
        AttackTarget();

        }
        lastTimesinceAttack += Time.deltaTime;
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
        if(!readyToAttack)
        {
            return;
        }
        
        
        if (Vector3.Distance(targetEnemy.transform.position, this.transform.position) < swordRange)
        {
            HandleSwordAttack();
        }
        else
        {
            //Debug.Log("attack with bow");
            HandleBowAttack();
        }
        lastTimesinceAttack = 0;
    }

    private void HandleBowAttack()
    {
        arrowInHand.gameObject.SetActive(true);
        SwordGJ.gameObject.SetActive(false);
        BowGJ.gameObject.SetActive(true);
        animator.SetTrigger("Bow");
    }

    private void HandleSwordAttack()
    {
        BowGJ.gameObject.SetActive(false);
        arrowInHand.gameObject.SetActive(false);
        SwordGJ.gameObject.SetActive(true);
        animator.SetTrigger("Attack1");
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
       
        targetEnemy = targetToHit;
        targetHealth = targetToHit.GetComponent<Health>();    
        if(targetHealth.IsDead())   
        return;
        targetVFX.transform.position = targetEnemy.transform.position+Vector3.up/8;
        targetVFX.gameObject.SetActive(true);
    }

    public Enemy GetTarget()
    {
      
        return targetEnemy;
    }
}

}