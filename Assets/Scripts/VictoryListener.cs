using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryListener : MonoBehaviour {
    public float winDistance = 5f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Kupa");
        
        if (gos.Length > 0 && Vector3.Distance(gos[0].transform.position, this.transform.position) < winDistance)
        {
            SceneManager.LoadSceneAsync("GameEnd");
            GameState.state = State.WIN;

        }
        
    }
}
