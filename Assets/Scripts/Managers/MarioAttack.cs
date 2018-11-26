using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioAttack : MonoBehaviour {
    public float attackDis = 4.9f;
    public float zAdjust = 1.8f;
    public float distanceToKupa;
    public float vertAdjust = 3f;
    private Vector3 kupaPos;
    private float kupaVel;
    private KupaState kupaState;

    private LifeController lc;
    public AudioSource injuredAudio;
    public float cooldown_time;
    private bool cooldown = false;
    private float cooldown_timer;

    /*private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Kupa") {
            Destroy(col.gameObject);
            BackToMainMenu();
        }
    }*/
    private void Start()
    {
        lc = GameObject.FindGameObjectWithTag("LifeController").GetComponent<LifeController>();
    }

    private void FixedUpdate()
    {
        GameObject kupa = GameObject.FindGameObjectWithTag("Kupa");
        if (kupa != null) {
            distanceToKupa = Vector3.Distance(kupa.transform.position, this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust));
            kupaPos = kupa.transform.position;
            kupaVel = kupa.GetComponent<PlayerMove>().velocityMag;
            kupaState = kupa.GetComponent<PlayerMove>().kupaState;
        } else
        {
            distanceToKupa = -1;
            kupaPos = new Vector3(0, 0, 0);
            kupaVel = 0f;
            kupaState = KupaState.NotSpinning;
        }

        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), (kupaPos - (this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust))).normalized * attackDis, Color.red);
        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), new Vector3(0, 1, 0) * 10f, Color.blue);


        if (kupa != null && (Vector3.Distance(kupaPos , this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust)) < attackDis))
        {
            if (kupaState == KupaState.Spinning && (kupaVel > 5f))
            {
                this.GetComponent<MarioController>().Stun();
            }
            else
            {
                if (!cooldown)
                {
                    lc.loseLife();
                    injuredAudio.Play();
                    cooldown = true;
                    cooldown_timer = cooldown_time;
                }
            }
        }
    }

    void Update()
    {
        if (cooldown)
        {
            cooldown_timer -= Time.deltaTime;
            if (cooldown_timer < 0)
            {
                cooldown = false;
                cooldown_timer = 0;
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("GameEnd");
		GameState.state = State.LOSS;
    }
}
