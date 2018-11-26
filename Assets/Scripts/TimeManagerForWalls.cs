using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerForWalls : MonoBehaviour {
    public float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
	}
}
