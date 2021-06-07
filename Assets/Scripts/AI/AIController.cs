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

    [SerializeField] bool attackPlayerByDefault = true;
    [SerializeField] List<string> tagToSearchTarget;

    [SerializeField] PatrolPath patrolPath = null;

    private AIFighter fighter;
    GameObject player;

    float timeSinceLastSeenPlayer = Mathf.Infinity;
    float timeSinceLastaggravatedTime = Mathf.Infinity;
    int currentWaypointIndex = 0;
    float timeScouting = 0;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float PatrolDwellTime = 5f;

    private Vector3 guardPosition;
    [SerializeField] float patrolSpeedfraction = 0.25f;
    private bool onlyDwellOnce = false;
    private bool reachedWaypoint = false;

    void Start()
    {        
        fighter = GetComponent<AIFighter>();

        if(attackPlayerByDefault){
            if(GameObject.FindGameObjectWithTag("Player")){ // null check

        player = GameObject.FindGameObjectWithTag("Player");
        fighter.SetTarget(player.transform);
            }
        }

        fighter.SetCollisionTag(tagToSearchTarget);
        guardPosition = transform.position;

    }

    void Update()
    {
        if (health.IsDead()) return;
      
        if (IsAggravated(fighter.GetTarget()) && fighter.CanAttack())
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
        
       
        if (patrolPath != null)
        {
            if (AtWaypoint(currentWaypointIndex) || reachedWaypoint == true)
            {
                reachedWaypoint = true;
                timeScouting += Time.deltaTime;
                if (timeScouting > PatrolDwellTime)
                {
                    reachedWaypoint = false;
                    timeScouting = 0;
                    CycleWaypoint(currentWaypointIndex);
                }else if(!onlyDwellOnce){
                    onlyDwellOnce = true;
                    mover.MoveToRandomDestinationInWayPoint(5f, 0.5f);
                     
                       
                }

            }else{
               onlyDwellOnce = false;
                    reachedWaypoint = false;
            }
              
            nextPosition = GetCurrentWaypoint();            
        }
        if(!reachedWaypoint)
        {
        mover.StartMoveAction(nextPosition, patrolSpeedfraction);
        }

    }


    private bool AtWaypoint(int index)
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void CycleWaypoint(int index)
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint(int index)
    {
        return patrolPath.GetWaypoint(index);
    }
    private bool IsAggravated(Transform targetToAggravate)
    {        
        if(targetToAggravate == null)
        {
            if(SearchTarget()){              
                return true;
            }else{
                return false;
            }
        }
        float distanceToTarget = Vector3.Distance(transform.position, targetToAggravate.position);
        if (distanceToTarget < chaseRange || distanceToTarget < fighter.getAttackRange())
        {
         
            Aggro();
            fighter.SetAggravated(true);
            return true;
        }
        if (timeSinceLastaggravatedTime < aggroCooldown)
        {
                //Debug.Log("cd ran out");
            fighter.SetAggravated(true);
            return true;
        }
        fighter.SetAggravated(false);
        return false;
    }

    private bool SearchTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, chaseRange, Vector3.up, 0f);
        foreach (RaycastHit hit in hits)
        {
            for (int i = 0; i < tagToSearchTarget.Count; i++)
            {
                
            if(hit.transform.gameObject.CompareTag(tagToSearchTarget[i]) && hit.transform.gameObject != this.gameObject)
            {
                fighter.SetTarget(hit.transform);
                Aggro();       
                return true;         
            }
            }
            
        }
        return false;
    }

    public void Aggro()
    {
        timeSinceLastaggravatedTime = 0f;
    }

    private void AttackBehaviour()
    {
        fighter.AttackCurrentTarget();       
    }
    public float getRange()
    {
        return chaseRange;
    }
}
}