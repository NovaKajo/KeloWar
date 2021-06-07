using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using Kelo.Stats;
using UnityEngine;
using UnityEngine.AI;


namespace Kelo.AI
{

    public class AIMover : MonoBehaviour,IAction
{
    [SerializeField] float maxSpeed = 5f;

    [SerializeField] private float stopDistance = 3f;
    NavMeshAgent navMesh;
    private Health health;

    private Vector3 navVelocity;
    private Vector3 localVelocity;
    private float forwardSpeed;


    private Scheduler scheduler;
    // Start is called before the first frame update
    void Awake()
    {
         scheduler = GetComponent<Scheduler>();
         navMesh = GetComponent<NavMeshAgent>();   
         health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsDead())
        {
            return;
        }
        CalculateForwardSpeed();
        
    }

    private void CalculateForwardSpeed()
    {
        navVelocity = navMesh.velocity; 
        localVelocity = transform.InverseTransformDirection(navVelocity);
        forwardSpeed = localVelocity.z; 
    }

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }

    public void StartMoveAction(Vector3 destination, float speedFraction)
    {
        scheduler.StartAction(this);
        navMesh.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        MoveTo(destination, speedFraction);
    }

    public void Disengage()
    {
       
        navMesh.isStopped = true;
    }

    public void MoveTo(Vector3 destination, float speedFraction)
    {
        navMesh.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        navMesh.isStopped = false;
        navMesh.SetDestination(destination);
        //navMesh.stoppingDistance = stopDistance;

    }

    public bool CanMoveTo(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
        if (!hasPath) return false;
        if (path.status != NavMeshPathStatus.PathComplete) return false;
       
        return true;
    }
}

}