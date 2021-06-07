using System;
using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using UnityEngine;

namespace Kelo.Player
{

public class PlayerAnimatorHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovementInput pmi;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float selectTargetTime = 0.2f;

    private Vector3 newLocation;
    private Scheduler scheduler;

    private float timeSinceStop;
    private bool changeOnMove = false;
    private PlayerAttack playerAttack;

    private Vector3 movementVector;

    private void Start() {
            
       scheduler = GetComponent<Scheduler>();
       playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable() {
        pmi.Move += HandleMovement;
    }
    private void OnDisable() {
        pmi.Move -= HandleMovement;
    }

    private void HandleMovement(Vector2 movement)
    {
        //animator.SetFloat("Horizontal",movement.x*movementSpeed);
        //animator.SetFloat("Vertical", movement.x != 0 ? Mathf.Abs(movement.x*movementSpeed) : Mathf.Abs(movement.y *movementSpeed));
        animator.SetFloat("Vertical",(Mathf.Abs(movement.x)+Mathf.Abs(movement.y))*movementSpeed);
    }


    // Update is called once per frame
    void Update()
    {
       
        if(pmi._move.x == 0 && pmi._move.y == 0)
        {
            timeSinceStop += Time.deltaTime;            
            if(timeSinceStop >=selectTargetTime && !changeOnMove)
            {
                EnemyList.FindClosestEnemy(this.transform);            
             playerAttack.SetTarget(EnemyList.closestEnemyToPlayer);
             playerAttack.readyToAttack = true;
             changeOnMove = true;
            }

            return;
        }else{
        playerAttack.readyToAttack = false;
        timeSinceStop = 0;
        playerAttack.Disengage();
    
        changeOnMove = false;
       
        movementVector = new Vector3(pmi._move.x, 0.0f, pmi._move.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), 0.15F);
        
      
        }
      
    }

   

    public void Disengage()
    {
        
    }
}
}
