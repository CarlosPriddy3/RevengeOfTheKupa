using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryListener : MonoBehaviour {
    public float winDistance = 5f;
    private GameObject kupa;
    private PlayerMove moveScript;
    private SoundManager soundManager;
    private Animator fader;
    private float disToKupa;

    // Use this for initialization
    void Start () {
        kupa = GameObject.FindGameObjectWithTag("Kupa");
        moveScript = kupa.GetComponent<PlayerMove>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Kupa");

        disToKupa = (this.transform.position - kupa.transform.position).magnitude;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (disToKupa < winDistance && moveScript.kupaState == KupaState.Spinning && moveScript.velocityMag > 3)
        {
            soundManager.fadeOut();
            fader.SetTrigger("win");
        }
    }
}
