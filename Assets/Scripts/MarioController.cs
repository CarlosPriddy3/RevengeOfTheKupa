﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Canvas marioStartledCanvas;
    public Canvas kupaStartledCanvas;
    private Text kupaStartledText;
    private static int marioCounter = 0;
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

        marioStartledCanvas.enabled = false;
        kupaStartledCanvas.enabled = false;

        kupaStartledText = kupaStartledCanvas.GetComponent<Text>();

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
                        if (disToTarget <= 50) // agent close enough to Koopa, chase Koopa
                        {
                            if (canSeeKupa())
                            {
                                marioCounter++;
                                if (marioCounter == 1) {
                                    playKupaFoundSound();
                                }
                                marioStartledCanvas.enabled = true;
                                kupaStartledText.text = getExclamationStr();
                                kupaStartledCanvas.enabled = true;
                                Debug.Log(this.name + " CHASING " + movingTarget.name);
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
                        if (disToTarget > 50)
                        {
                            marioCounter--;
                            marioStartledCanvas.enabled = false;
                            kupaStartledCanvas.enabled = false;
                            aiState = AIState.Patrol;
                        }
                        if (canSeeKupa() == false)
                        {
                            marioCounter--;
                            marioStartledCanvas.enabled = false;
                            kupaStartledCanvas.enabled = false;
                            if (marioCounter == 1) {
                                playInvestigatingSound();
                            }
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
                        if (this.name == "FireMario")
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
                        marioCounter--;
                        marioStartledCanvas.enabled = false;
                        kupaStartledCanvas.enabled = false;
                        agent.enabled = true;
                        this.GetComponent<MarioAttack>().enabled = true;
                        float dist2 = (movingTarget.transform.position - agent.transform.position).magnitude;
                        anim.SetBool("NotStunned", true);
                        if (dist2 < 50f)
                        {
                            if (canSeeKupa())
                            {
                                marioCounter++;
                                if (marioCounter == 1)
                                {
                                    playKupaFoundSound();
                                }
                                marioStartledCanvas.enabled = true;
                                kupaStartledText.text = getExclamationStr();
                                kupaStartledCanvas.enabled = true;
                                Debug.Log(this.name + " CHASING " + movingTarget.name);
                                aiState = AIState.Chase;
                            } else
                            {
                                if (marioCounter == 1) {
                                    playInvestigatingSound();
                                }
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

                    if (!agent.pathPending && agent.remainingDistance == 0 && movingTarget != null)
                    {
                        setNextWaypoint(investigationPoints);
                    }
                    
                    if (disToTarget <= 50) // agent close enough to Koopa, chase Koopa
                    {
                        if (canSeeKupa())
                        {
                            marioCounter++;
                            if (marioCounter == 1)
                            {
                                playKupaFoundSound();
                            }
                            marioStartledCanvas.enabled = true;
                            kupaStartledText.text = getExclamationStr();
                            kupaStartledCanvas.enabled = true;
                            Debug.Log(this.name + " CHASING " + movingTarget.name);
                            if (this.name == "FireMario")
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

    private string getExclamationStr() {
        string exclamationStr = "";
        for (int i = 0; i < marioCounter; i++)
        {
            exclamationStr += "!";
        }
        return exclamationStr;
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
        Debug.DrawRay(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized * (disToTarget - 4f), Color.cyan);
        bool objectBetween = Physics.Raycast(this.transform.position + adjVect, (movingTarget.transform.position - (this.transform.position + adjVect)).normalized, out hit, disToTarget - 4f) && hit.transform.name != "SupaKupaTrupa";
        Vector3 rayDir = (movingTarget.transform.position - (this.transform.position + adjVect)).normalized;
        if (Vector3.Angle(this.transform.forward, rayDir) > fieldOfView)
        {
            objectBetween = true;
        }
        return !objectBetween;
    }
    private void shootFireball()
    {
        /*float lookAhead = Mathf.Clamp(disToTarget, minLook, maxLook) / agent.speed;
        Vector3 target = targetPos + (lookAhead * kupaVel);
        bool blocked = NavMesh.Raycast(targetPos, target, out hit, NavMesh.AllAreas);
        Debug.DrawLine(transform.position, target, blocked ? Color.red : Color.green);
        if (blocked)
        {
            Vector3 tempDest = hit.position + (targetPos - hit.position).normalized * 1.3f;
            target = tempDest;
            Debug.DrawRay((hit.position + (targetPos - hit.position).normalized), Vector3.up, Color.blue);
        }*/

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
        GameObject[] searchPoints = new GameObject[5];
        //Initial search at location
        GameObject firstWayP = new GameObject();
        firstWayP.transform.position = new Vector3(location.x, 0, location.z);
        searchPoints[0] = firstWayP;
        for (int i = 1; i < searchPoints.Length; i++)
        {
            GameObject newWayp = new GameObject();
            newWayp.transform.position = location + new Vector3(Random.Range(0f, 20f), 0f, Random.Range(0f, 20f));
            int counter = 0;
            NavMeshHit hit;
            while (!NavMesh.Raycast(location, newWayp.transform.position, out hit, NavMesh.AllAreas))
            {
                newWayp.transform.position = location + new Vector3(Random.Range(0f, 20f), 0f, Random.Range(0f, 20f));
            }
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
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.forward * 400 + collision.gameObject.transform.up * 1000);
                            this.Stun();
                            
                        }
                        break;
                }
            }
        }
    }
}
