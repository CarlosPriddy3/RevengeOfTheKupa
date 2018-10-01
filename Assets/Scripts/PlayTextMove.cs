using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTextMove : MonoBehaviour {
    public GameObject textObj;
    public float rotateSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        textObj.transform.Rotate(rotateSpeed * Time.deltaTime, rotateSpeed * Time.deltaTime, 0);
	}
}
