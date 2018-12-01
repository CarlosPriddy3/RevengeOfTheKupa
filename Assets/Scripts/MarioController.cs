using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class MarioController : MonoBehaviour
{

    public enum AIState
    {
        Patrol,
        Chase,
        Investigate,
        Stunned
    };

    public AIState aiState;

    public GameObject[] waypoints;
    public int currWaypoint;
    public NavMeshAgent agent;
    private Animator anim;

    public float sightDistance = 70f;
    public float lookRotSpeed = 200f;
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
    //Fire Mario Specific
    public GameObject fireball; // SET IN INSPECTOR
    public float fireballCD = 300f;
    public int fireBallMaxRandom = 120;
    private int timer;
    public int searchTimer;
    public int searchTime = 3000;
    public GameObject[] investigationPoints;

    public AudioSource mammaMiaClip;
    public AudioSource hmmmmClip;

    private Canvas marioStartledCanvas;

    private Canvas kupaStartledCanvas;

    // Use this for initialization
    void Start () {
        movingTarget = GameObject.FindGameObjectWithTag("Kupa");
        aiState = AIState.Patrol;
        stunnedTimer = 0f;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currWaypoint = -1;
        setNextWaypoint(waypoints);
        timer = 0;
        searchTimer = 0;
        kupaStartledCanvas = GameObject.FindGameObjectWithTag("KupaStartledCanvas").GetComponent<Canvas>();
        marioStartledCanvas = this.GetComponentInChildren<Canvas>();
        marioStartledCanvas.enabled = false;
        kupaStartledCanvas.enabled = false;

        kupaVel = movingTarget.GetComponent<PlayerMove>().velocity;
    }

    // Update is called once per frame
    void Update () {
        if (!GameState.paused)
        {
            
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
                    //Line of Sight Debug Rays
                    Quaternion rightRot = Quaternion.AngleAxis(fieldOfView, Vector3.up);
                    Quaternion leftRot = Quaternion.AngleAxis(-fieldOfView, Vector3.up);
                    Vector3 rightVec = rightRot * transform.forward;
                    Vector3 leftVec = leftRot * transform.forward;
                    Debug.DrawRay(this.transform.position, rightVec * 20f, Color.green);
                    Debug.DrawRay(this.transform.position, leftVec * 20f, Color.green);

                    if (movingTarget != null)
                    {
                        if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null)
                        {
                            setNextWaypoint(waypoints);
                        }
                        RaycastHit hit;
                        if (disToTarget <= sightDistance) // agent close enough to Koopa, chase Koopa
                        {
                            if (canSeeKupa())
                            {
                                playKupaFoundSound();
                                marioStartledCanvas.enabled = true;
                                kupaStartledCanvas.enabled = true;
                                if (this.name == "FireMario")
                                {
                                    shootFireball();
                                }
                                aiState = AIState.Chase;
                                break;
                            }

                        }
                    }

                    break;

                case AIState.Chase:
                    if (movingTarget != null)
                    {
                        if (disToTarget > sightDistance)
                        {
                            marioStartledCanvas.enabled = false;
                            kupaStartledCanvas.enabled = false;
                            aiState = AIState.Patrol;
                        }
                        if (canSeeKupa() == false)
                        {
                            marioStartledCanvas.enabled = false;
                            kupaStartledCanvas.enabled = false;
                            playInvestigatingSound();
                            InstantiateInvestigateParams(targetPos);
                            break;
                        }
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
                        
                        if (this.tag == "StationaryFireMario")
                        {
                            Debug.Log("NO NAV PATH FOUND");
                            //find the vector pointing from our position to the target
                            Vector3 dir = (targetPos - transform.position).normalized;

                            //create the rotation we need to be in to look at the target
                            Quaternion lookRot = Quaternion.LookRotation(dir);

                            //rotate us over time according to speed until we are in the required rotation
                            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * lookRotSpeed);
                        }
                        if (this.tag == "FireMario" || this.tag == "StationaryFireMario")
                        {
                            timer++;
                            if (timer > fireballCD)
                            {
                                shootFireball();
                                timer = Random.Range(0, fireBallMaxRandom);
                            }
                        }  
                    }
                    break;
                case AIState.Stunned:
                    stunnedTimer += Time.deltaTime;
                    if (stunnedTimer > stunDuration)
                    {
                        marioStartledCanvas.enabled = false;
                        kupaStartledCanvas.enabled = false;
                        agent.enabled = true;
                        this.GetComponent<MarioAttack>().enabled = true;
                        float dist2 = (movingTarget.transform.position - agent.transform.position).magnitude;
                        anim.SetBool("NotStunned", true);
                        if (dist2 < sightDistance)
                        {
                            if (canSeeKupa())
                            {
                                marioStartledCanvas.enabled = true;
                                kupaStartledCanvas.enabled = true;

                                Debug.Log(this.name + " CHASING " + movingTarget.name);
                                aiState = AIState.Chase;
                            } else
                            {
                                playInvestigatingSound();
                                InstantiateInvestigateParams(this.transform.position);
                                aiState = AIState.Investigate;
                            }       
                        } else
                        {
                            aiState = AIState.Patrol;
                        }
                    }
                    break;
                case AIState.Investigate:
                    searchTimer++;
                    
                    
                    if (searchTimer > searchTime)
                    {
                        Debug.Log("TIME UP COULDNT FIND");
                        clearInvestigationPoints();
                        aiState = AIState.Patrol;
                        break;
                    }

                    if (!agent.pathPending && agent.remainingDistance < 2 && movingTarget != null)
                    {
                        setNextWaypoint(investigationPoints);
                    }
                    
                    if (disToTarget <= 50) // agent close enough to Koopa, chase Koopa
                    {
                        if (canSeeKupa())
                        {
                            marioStartledCanvas.enabled = true;
                            kupaStartledCanvas.enabled = true;
                            playKupaFoundSound();
                            if (this.tag == "FireMario")
                            {
                                shootFireball();
                            }
                            
                            aiState = AIState.Chase;
                            break;
                        }

                    }
                    break;
            }
        }
    }

    private void playKupaFoundSound() {
        mammaMiaClip.Play();
    }

    private void playInvestigatingSound() {
        hmmmmClip.Play();
    }

    private void setNextWaypoint(GameObject[] waypoints) {
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

    private bool canSeeKupa()
    {
        RaycastHit hit;
        Vector3 adjVect = this.transform.forward * 2 + this.transform.up * 5;
        bool objectBetween;
        if (movingTarget != null)
        {
            Debug.DrawRay(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized * (disToTarget - 4f), Color.cyan);
            objectBetween = Physics.Raycast(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized, out hit, disToTarget - 4f) && hit.transform.name != "SupaKupaTrupa";
            Vector3 rayDir = (movingTarget.transform.position - (this.transform.position + adjVect)).normalized;
            if (Vector3.Angle(this.transform.forward, rayDir) > fieldOfView)
            {
                objectBetween = true;
            }
        } else
        {
            objectBetween = true;
        }
        
        return !objectBetween;
    }
    private void shootFireball()
    {
        GameObject instance = Instantiate(fireball);
        instance.transform.position = gameObject.transform.position + (gameObject.transform.forward * 5) + (gameObject.transform.up * 5);
        instance.GetComponent<Rigidbody>().AddForce((this.transform.forward.normalized * 60) + (Vector3.down * 170 / disToTarget), ForceMode.Impulse);
    }

    //Helper Methods for Investigation of Sound
    public void InstantiateInvestigateParams(Vector3 location)
    {
        clearInvestigationPoints();
        searchTimer = 0;
        currWaypoint = 0;
        generateSearchPoints(location);
        agent.SetDestination(investigationPoints[0].transform.position);
        aiState = AIState.Investigate;
    }

    private void generateSearchPoints(Vector3 location)
    {
        Vector3 xzLocation = new Vector3(location.x, 0, location.z);
        GameObject[] searchPoints = new GameObject[5];
        //Initial search at location
        GameObject firstWayP = new GameObject();
        firstWayP.transform.position = new Vector3(location.x, 0, location.z);
        searchPoints[0] = firstWayP;
        for (int i = 1; i < searchPoints.Length; i++)
        {
            GameObject newWayp = new GameObject();
            newWayp.transform.position = xzLocation + new Vector3(Random.Range(0f, 40f) - 20, 0f, Random.Range(0f, 40f) - 20);
            int counter = 0;
            NavMeshHit hit;
            int whileCounter = 0;
            RaycastHit physicsHit;
            bool objectBetween = Physics.Raycast(xzLocation + new Vector3(0, 3, 0), (newWayp.transform.position - xzLocation).normalized, out physicsHit) && physicsHit.transform.name != "SupaKupaTrupa";
            while (NavMesh.Raycast(xzLocation, newWayp.transform.position, out hit, NavMesh.AllAreas) && objectBetween && whileCounter < 50)
            {
                newWayp.transform.position = xzLocation + new Vector3(Random.Range(0f, 40f) - 20, 0f, Random.Range(0f, 40f) - 20);
                objectBetween = Physics.Raycast(xzLocation + new Vector3(0, 3, 0), (newWayp.transform.position - xzLocation).normalized, out physicsHit) && physicsHit.transform.name != "SupaKupaTrupa";
                whileCounter++;
            }
            //Debug for search locations
           // Debug.DrawRay(xzLocation + new Vector3(0, 3, 0), (newWayp.transform.position - xzLocation).normalized * 20, Color.black, 300f);
            searchPoints[i] = newWayp;
        }
        this.investigationPoints = searchPoints;
    }

    public void clearInvestigationPoints()
    {
        foreach (GameObject point in investigationPoints)
        {
            Destroy(point);
        }
    }

    //Head collision checker
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
                            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.forward * 400 + collision.gameObject.transform.up * 1000);
                            this.Stun();
                            
                        }
                        break;
                }
            }
        }
    }
}
