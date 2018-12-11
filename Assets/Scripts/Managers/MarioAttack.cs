using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarioAttack : MonoBehaviour {
    public float attackDis = 4.9f;
    public float zAdjust = 1.8f;
    public float distanceToKupa;
    public float vertAdjust = 3f;
    private Vector3 kupaPos;
    public float kupaVel;
    private KupaState kupaState;

    private LifeController lc;
  
    public AudioSource injuredAudio;
    public float cooldown_time;
    private bool cooldown = false;
    private float cooldown_timer;

    private Image damageImage;
    public float flashSpeed = 2f;
    public Color flashColour = new Color(1f, 0f, 0f, 1f);

    /*private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Kupa") {
            Destroy(col.gameObject);
            BackToMainMenu();
        }
    }*/
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("LifeController") != null)
        {
            lc = GameObject.FindGameObjectWithTag("LifeController").GetComponent<LifeController>();
        }
        if (GameObject.FindGameObjectWithTag("DamageImage") != null)
        {
            damageImage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<Image>();
        }
        
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
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, Time.deltaTime * 3f);

        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), (kupaPos - (this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust))).normalized * attackDis, Color.red);
        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), new Vector3(0, 1, 0) * 10f, Color.blue);


        if (kupa != null && (Vector3.Distance(kupaPos , this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust)) < attackDis))
        {
            if (kupaState == KupaState.Spinning && (kupaVel > 5f))
            {
                this.GetComponent<MarioController>().Stun();
                Rigidbody rb = kupa.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(kupa.transform.forward * -1200f);
            }

            if (!cooldown)
            {   
                lc.loseLife();
                Debug.Log("Lost a life");
                if (lc.getNumLives() > 0)
                {
                    damageImage.color = flashColour;
                } else
                {
                    damageImage.color = Color.red;
                    damageImage.color = new Color(0, 0, 0, 0.8f);
                }
                injuredAudio.Play();
                cooldown = true;
                cooldown_timer = cooldown_time;
            }
            Debug.Log("You have " + lc.getNumLives() + " lives left");
            
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
