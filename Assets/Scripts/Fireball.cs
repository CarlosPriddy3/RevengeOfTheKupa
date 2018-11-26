using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fireball : MonoBehaviour {
    private LifeController lc;
    public AudioSource injuredAudio;
    private int timer = 0;

    private void Start()
    {
        lc = GameObject.FindGameObjectWithTag("LifeController").GetComponent<LifeController>();
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
            injuredAudio.Play();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("GameEnd");
        GameState.state = State.LOSS;
    }
}
