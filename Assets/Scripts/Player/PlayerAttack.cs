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
    public GameObject targetGJ;

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

    public List<GameObject> gos; 
    
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

    public void FindClosestEnemy()
    {
       
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if(go.activeSelf == false)
            {
               continue;
            }
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                targetGJ = go;
                distance = curDistance;
            }
           
        }
    }

    public void LookAtTarget()
    {
        
        if (targetGJ == null)
        {
            return;
        }

        // Smoothly rotate towards the target point.
        var targetRotation = Quaternion.LookRotation(targetGJ.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
       
    }

    private void AttackTarget()
    {
        

        if (targetGJ == null)
        {
            Debug.Log("no target.." );
            return;
        }
        scheduler.StartAction(this);
        
        if (Vector3.Distance(targetGJ.transform.position, this.transform.position) < swordRange)
        {
            HandleSwordAttack();
        }
        else
        {
            Debug.Log("attack with bow");
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
        SwordGJ.gameObject.SetActive(true);
        animator.SetTrigger("Attack1");
    }

    public void DamageTarget()
    {
      targetGJ.GetComponent<Health>().TakeDamage(-5);
    }

    public void Disengage()
    {
        targetGJ = null;
    }
}

}