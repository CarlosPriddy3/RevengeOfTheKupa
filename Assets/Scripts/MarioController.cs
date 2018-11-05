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
        Chase,
        Stunned
    };

    public AIState aiState;
    
    public GameObject[] waypoints;
    public int currWaypoint;
    private NavMeshAgent agent;
    private Animator anim;

    private GameObject movingTarget;
    private Vector3 kupaVel;
    public float stunDuration = 3f;
    private float stunnedTimer;
    public float minLook = .1f;
    public float maxLook = 4f;
    private NavMeshHit hit;

    // Use this for initialization
    void Start () {
        movingTarget = GameObject.FindGameObjectWithTag("Kupa");
        aiState = AIState.Patrol;
        stunnedTimer = 0f;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currWaypoint = -1;
        setNextWaypoint();

        kupaVel = movingTarget.GetComponent<PlayerMove>().velocity;
    }

    // Update is called once per frame
    void Update () {
        if (!GameState.paused)
        {
            switch (aiState)
            {
                case AIState.Patrol:
                    float dist1 = (movingTarget.transform.position - agent.transform.position).magnitude;
                    if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null) {                          
                        setNextWaypoint();
                    }
                    if (dist1 <= 50) // agent close enough to Koopa, chase Koopa
                    {
                        aiState = AIState.Chase;
                        break;
                    }
                    break;

                case AIState.Chase:
                    if(movingTarget != null)
                    {
                        /*
                        float dist2 = (movingTarget.transform.position - agent.transform.position).magnitude;
                        float speed = (agent.speed == 0) ? 1 : agent.speed;
                        float lookAheadT = dist2 / agent.speed;
                        Vector3 futureTarget = movingTarget.transform.position + lookAheadT * kupaVel;
                        agent.SetDestination(futureTarget);

                        if (dist2 > 50) // agent far enough from Koopa, go back to patrolling
                        {
                            aiState = AIState.Patrol;
                            setNextWaypoint();
                        }*/
                        kupaVel = movingTarget.GetComponent<PlayerMove>().velocity;
                        Vector3 targetPos = movingTarget.transform.position;
                        float disToTarget = Vector3.Magnitude(targetPos - this.transform.position);
                        float lookAhead = Mathf.Clamp(disToTarget, minLook, maxLook) / agent.speed;
                        Vector3 dest = targetPos + (lookAhead * kupaVel);
                        bool blocked = NavMesh.Raycast(targetPos, dest, out hit, NavMesh.AllAreas);
                        Debug.DrawLine(transform.position, dest, blocked ? Color.red : Color.green);
                        if (blocked)
                        {
                            Vector3 tempDest = hit.position + (targetPos - hit.position).normalized * 1.3f;
                            agent.SetDestination(tempDest);
                            Debug.DrawRay((hit.position + (targetPos - hit.position).normalized), Vector3.up, Color.blue);
                        }
                        else
                        {
                            agent.SetDestination(dest);
                        }
                    }
                    break;
                case AIState.Stunned:
                    stunnedTimer += Time.deltaTime;
                    if (stunnedTimer > stunDuration)
                    {
                        agent.enabled = true;
                        this.GetComponent<MarioAttack>().enabled = true;
                        float dist2 = (movingTarget.transform.position - agent.transform.position).magnitude;
                        anim.SetBool("NotStunned", true);
                        if (dist2 < 50f)
                        {
                            aiState = AIState.Chase;
                        } else
                        {
                            aiState = AIState.Patrol;
                        }
                    }
                    break;
            }
        }
    }

    private void setNextWaypoint() {
        if (waypoints.Length == 0) {
            return;
        }
        currWaypoint = (currWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currWaypoint].transform.position);
    }

    public void Stun()
    {
        agent.enabled = false;
        aiState = AIState.Stunned;
        anim.Play("MarioDizzy");
        stunnedTimer = 0f;
        anim.SetBool("NotStunned", false);
        this.GetComponent<MarioAttack>().enabled = false;
        
    }
}
