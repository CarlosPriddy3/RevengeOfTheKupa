using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioAttack : MonoBehaviour {

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Kupa") {
            Destroy(col.gameObject);
            BackToMainMenu();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
