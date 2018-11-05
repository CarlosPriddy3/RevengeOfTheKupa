using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fireball : MonoBehaviour {

    private int timer = 0;

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
            Destroy(collision.gameObject);
            BackToMainMenu();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("GameEnd");
        GameState.state = State.LOSS;
    }
}
