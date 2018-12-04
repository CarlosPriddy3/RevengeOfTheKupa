using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorOrentation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("floor");
        foreach (GameObject floor in floors)
        {
            int random = Random.Range(1, 5);
            floor.transform.Rotate(new Vector3(0, 90 * random, 0));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
