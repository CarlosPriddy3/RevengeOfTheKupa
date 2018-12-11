using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fireball : MonoBehaviour {
    private LifeController lc;
    public AudioSource injuredAudio;
    private int timer = 0;
    private Image damageImage;

    private void Start()
    {
        lc = GameObject.FindGameObjectWithTag("LifeController").GetComponent<LifeController>();
        if (GameObject.FindGameObjectWithTag("DamageImage") != null)
        {
            damageImage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<Image>();
        }
    }
    private void Update()
    {
        timer++;
        if (timer > 100)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Kupa")
        {
            lc.loseLife();
            if (lc.getNumLives() > 0)
            {
                damageImage.color = new Color(1, 0, 0, 1f);
            }
            else
            {
                damageImage.color = Color.red;
                damageImage.color = new Color(0, 0, 0, 0.7f);
            }
            injuredAudio.Play();
            Destroy(this.gameObject);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("GameEnd");
        GameState.state = State.LOSS;
    }
}
