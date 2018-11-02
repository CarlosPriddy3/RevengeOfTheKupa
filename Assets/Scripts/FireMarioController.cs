using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class FireMarioController : MonoBehaviour
{

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

    public GameObject fireball;     // SET IN INSPECTOR
    private int timer = 0;

    // Use this for initialization
    void Start()
    {
        aiState = AIState.Patrol;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currWaypoint = -1;
        setNextWaypoint();

        movingTargetScript = movingTarget.GetComponent<VelocityReporter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.paused)
        {
            switch (aiState)
            {
                case AIState.Patrol:
                    Debug.Log("Patrol");
                    if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null)
                    {
                        float dist1 = (movingTarget.transform.position - transform.position).magnitude;
                        //Debug.Log(dist1);
                        if (dist1 <= 50) // agent close enough to Koopa, chase Koopa
                        {
                            shootFireball(movingTarget.transform.position - transform.position);
                            aiState = AIState.Chase;
                            break;
                        }
                        setNextWaypoint();
                    }
                    break;

                case AIState.Chase:
                    Debug.Log("Chase");
                    if (movingTarget != null)
                    {
                        float dist2 = (movingTarget.transform.position - agent.transform.position).magnitude;
                        float lookAheadT = dist2 / agent.speed;
                        Vector3 futureTarget = movingTarget.transform.position + lookAheadT * movingTargetScript.velocity;
                        agent.SetDestination(futureTarget);

                        if (dist2 > 50) // agent far enough from Koopa, go back to patrolling
                        {
                            Debug.Log("Hello?");
                            aiState = AIState.Patrol;
                            setNextWaypoint();
                        }

                        timer++;
                        if (timer > 50)
                        {
                            shootFireball(movingTarget.transform.position - transform.position);
                            timer = 0;
                        }
                    }
                    break;
            }
        }
    }

    private void setNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return;
        }
        currWaypoint = (currWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currWaypoint].transform.position);
    }

    private void shootFireball(Vector3 target)
    {
        // change to object pooling later?
        Debug.Log("Shoot Fireball");
        GameObject instance = Instantiate(fireball);
        instance.transform.position = gameObject.transform.position;
        instance.GetComponent<Rigidbody>().AddForce((target.normalized * 100) + (Vector3.down * 40), ForceMode.Impulse);
    }
}
