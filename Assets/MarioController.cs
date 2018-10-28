using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour {

    public enum AIState
    {
        Patrol,
        Chase
    };

    public AIState aiState;

    public GameObject[] waypoints;
    public int currWaypoint;
    public NavMeshAgent agent;
    public Animator anim;

    public GameObject movingTarget;
    public VelocityReporter movingTargetScript; 

    // Use this for initialization
    void Start () {
        aiState = AIState.Patrol;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currWaypoint = 0;
        setNextWaypoint();

        movingTargetScript = movingTarget.GetComponent<VelocityReporter>();
    }
    
    // Update is called once per frame
    void Update () {
        switch (aiState)
        {            
            case AIState.Patrol:
                if (!agent.pathPending && agent.remainingDistance == 0) {
                    currWaypoint = (currWaypoint + 1) % waypoints.Length;

                    //if (currWaypoint == waypoints.Length - 1)
                    //{
                    //    aiState = AIState.GoToMovingWaypoint;
                    //    break;
                    //}                  

                    setNextWaypoint();
                }
                break;

            case AIState.Chase:
                float dist = (waypoints[currWaypoint].transform.position - agent.transform.position).magnitude;
                float lookAheadT = dist / agent.speed;
                //Vector3 futureTarget = waypoints[currWaypoint].transform.position + lookAheadT * movingTargetScript.velocity;
                //agent.SetDestination(futureTarget);

                //if (!agent.pathPending && agent.remainingDistance < 0.25f)
                //{
                //    currWaypoint = (currWaypoint + 1) % waypoints.Length;
                //    aiState = AIState.GoToStaticWaypoint;
                //    setNextWaypoint();
                //}

                break;
        }
        anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
    }

    private void setNextWaypoint() {
        if (waypoints.Length == 0) {
            return;
        }

        agent.SetDestination(waypoints[currWaypoint].transform.position);
    }
}
