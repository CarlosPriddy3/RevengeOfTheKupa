using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    //public int startingHealth = 100;
    //public int currentHealth;
    //public Slider healthSlider;
    private LifeController lifeController;
    public bool isCoolDown = false;            //true means can't get hurt again
    public int cooldownThreshold;
    public int timer;

    //Animator anim;
    public AudioSource injuredAudio;
    //PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    //bool isDead;
    //bool damaged;

    void Awake()
    {
        //anim = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
        //playerMovement = GetComponent<PlayerMovement>();
        //playerShooting = GetComponentInChildren<PlayerShooting>();
        //currentHealth = startingHealth;
        lifeController = GameObject.FindGameObjectWithTag("LifeController").GetComponent<LifeController>();
        cooldownThreshold = 200;
        timer = 0;
        injuredAudio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mario" && !isCoolDown)
        {
            Debug.Log("COOLDOWN START");
            lifeController.loseLife();
            isCoolDown = true;
            injuredAudio.Play();
        }
    }

    void Update()
    {
        if (isCoolDown)
        {
            timer++;
            if (timer > cooldownThreshold)
            {
                Debug.Log("COOLDOWN END");
                timer = 0;
                isCoolDown = false;
            }
        }
        //if (damaged)
        //{
        //    damageimage.color = flashcolour;
        //}
        //else
        //{
        //    damageimage.color = color.lerp(damageimage.color, color.clear, flashspeed * time.deltatime);
        //}
        //damaged = false;
    }


    //public void TakeDamage(int amount)
    //{
    //    damaged = true;

    //    currentHealth -= amount;

    //    //healthSlider.value = currentHealth;
        
    //    if (currentHealth <= 0 && !isDead)
    //    {
    //        Death();
    //    }
    //}


    //void Death()
    //{
    //    isDead = true;
    //}


    //public void RestartLevel()
    //{
    //    SceneManager.LoadScene(0);
    //}

}
