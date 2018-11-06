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
    private float disToTarget;
    private Vector3 targetPos;
    public float fieldOfView = 45f;

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
            Quaternion rightRot = Quaternion.AngleAxis(fieldOfView, Vector3.up);
            Quaternion leftRot = Quaternion.AngleAxis(-fieldOfView, Vector3.up);
            Vector3 rightVec = rightRot * transform.forward;
            Vector3 leftVec = leftRot * transform.forward;
            Debug.DrawRay(this.transform.position, rightVec * 20f, Color.green);
            Debug.DrawRay(this.transform.position, leftVec * 20f, Color.green);
            if (movingTarget != null)
            {
                kupaVel = movingTarget.GetComponent<PlayerMove>().velocity - (new Vector3(0, movingTarget.GetComponent<PlayerMove>().velocity.y, 0));
                targetPos = movingTarget.transform.position - (new Vector3(0, movingTarget.transform.position.y, 0));
                disToTarget = Vector3.Magnitude(targetPos - this.transform.position);
            } else
            {
                kupaVel = new Vector3(0, 0, 0);
                targetPos = new Vector3(0, 0, 0);
                disToTarget = 0f;
            }
            

            switch (aiState)
            {
                case AIState.Patrol:
                    if (movingTarget != null)
                    {
                        if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null)
                        {
                            setNextWaypoint();
                        }
                        RaycastHit hit;
                        if (disToTarget <= 50) // agent close enough to Koopa, chase Koopa
                        {
                            Vector3 adjVect = this.transform.forward * 2 + this.transform.up * 5;
                            Debug.DrawRay(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized * (disToTarget - 4f), Color.cyan);
                            bool objectBetween = Physics.Raycast(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized, out hit, disToTarget - 4f) && hit.transform.name != "SupaKupaTrupa";
                            Vector3 rayDir = (movingTarget.transform.position - (this.transform.position + adjVect)).normalized;
                            if (Vector3.Angle(this.transform.forward, rayDir) > fieldOfView)
                            {
                                objectBetween = true;
                            }
                           // Debug.Log(objectBetween);
                            if (!objectBetween)
                            {
                                Debug.Log("IN STATE CHANGE");
                                aiState = AIState.Chase;
                                break;
                            }
                            
                        }
                    }
                    
                    break;

                case AIState.Chase:
                    if(movingTarget != null)
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Kupa")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                string colName = contact.thisCollider.name;
                switch (colName)
                {
                    case "HeadCollider":
                        Vector3 relativePosition = contact.thisCollider.transform.InverseTransformPoint(contact.point);
                        if (relativePosition.y > 0 && kupaVel.y <= 0)
                        {
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.forward * 400 + collision.gameObject.transform.up * 1000);
                            this.Stun();
                            
                        }
                        break;
                }
            }
        }
    }
}
