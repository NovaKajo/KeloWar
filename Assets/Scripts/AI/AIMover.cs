using System.Collections;
using System.Collections.Generic;
using Kelo.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Kelo.AI
{

    public class AIMover : MonoBehaviour,IAction
{
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] Animator myAnim;

    [SerializeField] private float stopDistance = 3f;
    NavMeshAgent navMesh;
    // Start is called before the first frame update
    void Awake()
    {
        
         navMesh = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {

        Animator();

    }

    private void Animator()
    {
        Vector3 velocity = navMesh.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        //myAnim.SetFloat("forwardSpeed", speed);
    }

    public void StartMoveAction(Vector3 destination, float speedFraction)
    {
        GetComponent<Scheduler>().StartAction(this);
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