using System.Collections;
using System.Collections.Generic;
using Kelo.Stats;
using UnityEngine;

namespace Kelo.AI
{


public class AIController : MonoBehaviour
{
    [SerializeField] float chaseRange = 5f;

    [SerializeField] float aggroCooldown = 10f;

    [SerializeField] Health health;
    [SerializeField] AIMover mover;
    private AIFighter fighter;
    GameObject player;

    float timeSinceLastSeenPlayer = Mathf.Infinity;
    float timeSinceLastaggravatedTime = Mathf.Infinity;

    private Vector3 guardPosition;
    [SerializeField] float patrolSpeedfraction = 0.25f;
    


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fighter = GetComponent<AIFighter>();
        guardPosition = transform.position;
    }

    void Update()
    {
        if (health.IsDead()) return;
        if (IsAggravated() && fighter.CanAttack(player))
        {

            timeSinceLastSeenPlayer = 0;           
            AttackBehaviour();
        }
        else
        {
            //Debug.Log("Returning.. to guard position" + gameObject.name);
            PatrolBehaviour();
        }
        timeSinceLastSeenPlayer += Time.deltaTime;
        timeSinceLastaggravatedTime += Time.deltaTime;
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition;

        /*
        if (patrolPath != null)
        {
            if (AtWaypoint(currentWaypointIndex))
            {
                timeScouting += Time.deltaTime;
                if (timeScouting > PatrolDwellTime)
                {
                    timeScouting = 0;
                    CycleWaypoint(currentWaypointIndex);
                }

            }
            nextPosition = GetCurrentWaypoint();



        }
        */
        mover.StartMoveAction(nextPosition, patrolSpeedfraction);
    }
    private bool IsAggravated()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (timeSinceLastaggravatedTime < aggroCooldown)
        {
            Debug.Log("cd ran out");
            return true;
        }
        if (distanceToPlayer < chaseRange)
        {
          Debug.Log("player is close chasing");
            Aggro();
            return true;
        }
        return false;
    }

    public void Aggro()
    {
        timeSinceLastaggravatedTime = 0f;
    }

    private void AttackBehaviour()
    {
        fighter.Attack(player);
        
    }
    public float getRange()
    {
        return chaseRange;
    }
}
}