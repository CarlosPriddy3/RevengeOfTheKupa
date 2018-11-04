using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioAttack : MonoBehaviour {
    public float attackDis = 4.9f;
    public float zAdjust = 1.8f;
    public float distanceToKupa;
    public float vertAdjust = 3f;
    /*private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Kupa") {
            Destroy(col.gameObject);
            BackToMainMenu();
        }
    }*/

    private void FixedUpdate()
    {
        GameObject kupa = GameObject.FindGameObjectWithTag("Kupa");
        distanceToKupa = Vector3.Distance(kupa.transform.position, this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust));
        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), (kupa.transform.position - (this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust))).normalized * attackDis, Color.red);
        Debug.DrawRay(transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust), new Vector3(0, 1, 0) * 10f, Color.blue);
        KupaState kupaState = kupa.GetComponent<PlayerMove>().kupaState;
        float kupaVel = kupa.GetComponent<PlayerMove>().velocityMag;
        if (kupa != null && (Vector3.Distance(kupa.transform.position , this.transform.position + (this.transform.forward * zAdjust) + (this.transform.up * vertAdjust)) < attackDis))
        {
            if (kupa.GetComponent<PlayerMove>().kupaState == KupaState.Spinning && (kupaVel > 5f))
            {
                this.GetComponent<MarioController>().Stun();
            } else
            {
                BackToMainMenu();
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("GameEnd");
		GameState.state = State.LOSS;
    }
}
