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

        if (gos.Length > 0 && Vector3.Distance(gos[0].transform.position, this.transform.position) < winDistance && moveScript.kupaState == KupaState.Spinning && moveScript.velocityMag > 3)
        {
            fader.SetTrigger("win");
        }

    }
}
