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
        Chase,
        Stunned
    };

    public AIState aiState;

    public GameObject[] waypoints;
    public int currWaypoint;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 kupaVel;
    public float minLook = .1f;
    public float maxLook = 4f;
    public float stunDuration = 3f;
    private float stunnedTimer;
    private NavMeshHit hit;
    private GameObject movingTarget;
    public VelocityReporter movingTargetScript;
    private Vector3 targetPos;
    private float disToTarget;

    public GameObject fireball;     // SET IN INSPECTOR
    private int timer = 0;

    // Use this for initialization
    void Start()
    {
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
    void Update()
    {
        if (!GameState.paused)
        {
            if (movingTarget != null)
            {
                kupaVel = movingTarget.GetComponent<PlayerMove>().velocity - (new Vector3(0, movingTarget.GetComponent<PlayerMove>().velocity.y, 0));
                targetPos = movingTarget.transform.position - (new Vector3(0, movingTarget.transform.position.y, 0));
                disToTarget = Vector3.Magnitude(targetPos - this.transform.position);
            }
            else
            {
                kupaVel = new Vector3(0, 0, 0);
                targetPos = new Vector3(0, 0, 0);
                disToTarget = 0f;
            }
            switch (aiState)
            {

                case AIState.Patrol:
                    if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null)
                    {
                        setNextWaypoint();
                    }
                    if (disToTarget <= 50) // agent close enough to Koopa, chase Koopa
                    {
                        shootFireball(movingTarget.transform.position - transform.position);
                        aiState = AIState.Chase;
                        break;
                    }
                    break;

                case AIState.Chase:
                    if (movingTarget != null)
                    {
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

                        timer++;
                        if (timer > 50)
                        {
                            shootFireball(movingTarget.transform.position - transform.position);
                            timer = 0;
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
                        }
                        else
                        {
                            aiState = AIState.Patrol;
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
        GameObject instance = Instantiate(fireball);
        instance.transform.position = gameObject.transform.position + (gameObject.transform.forward * 2);
        instance.GetComponent<Rigidbody>().AddForce((target.normalized * 160) + (Vector3.down * 100), ForceMode.Impulse);
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
