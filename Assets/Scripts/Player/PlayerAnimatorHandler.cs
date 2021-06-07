using System;
using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using Kelo.Enemies;
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
    private bool rollState;

    private int VerticalHash = Animator.StringToHash("Vertical");

    private void Start() {
            
       scheduler = GetComponent<Scheduler>();
       playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable() {
        pmi.Move += HandleMovement;
        pmi.Dash += HandleDash;
    }
    private void OnDisable() {
        pmi.Move -= HandleMovement;
        pmi.Dash -= HandleDash;
    }

    private void HandleMovement(Vector2 movement)
    {
       
        animator.SetFloat(VerticalHash,(Mathf.Abs(movement.x)+Mathf.Abs(movement.y))*movementSpeed);
    }
    private void HandleDash(bool dashState)
    {
       
        if(dashState == true && !animator.GetCurrentAnimatorStateInfo(2).IsName("Roll") )
        {
        animator.SetTrigger("Roll");
     
        }
     
    }


    // Update is called once per frame
    void Update()
    {
       
       if(pmi._isdashing == true || animator.GetCurrentAnimatorStateInfo(1).IsName("Roll"))
            {
                ResetTarget();
                if (pmi._move.x != 0 || pmi._move.y != 0)
                {
                    RotateToDirection(0.1f);
                }
                else if (pmi._move.x == 0 && pmi._move.y == 0)
                {
                    changeOnMove = false;
                }
                return;
            }
            else
            {
            if(pmi._move.x == 0 && pmi._move.y == 0)
            {
                timeSinceStop += Time.deltaTime;            
                if(timeSinceStop >=selectTargetTime && !changeOnMove)
                    {
                        FindNewEnemy();
                    }

                    return;
            }else{
            ResetTarget();
            changeOnMove = false;        
            RotateToDirection(0.15f);
            }      
        }        
    }

        private void RotateToDirection(float time)
        {
            movementVector = new Vector3(pmi._move.x, 0.0f, pmi._move.y);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), time);
        }

        private void ResetTarget()
        {
            playerAttack.Disengage();
            playerAttack.canAttack = false;
            timeSinceStop = 0f;
        }

        //probablemente esto no va aqui
        private void FindNewEnemy()
        {
            EnemyList.FindClosestEnemy(this.transform);
            playerAttack.SetTarget(EnemyList.closestEnemyToPlayer);
            playerAttack.canAttack = true;
            changeOnMove = true;
        }


        public void Disengage()
    {
        
    }
}
}
