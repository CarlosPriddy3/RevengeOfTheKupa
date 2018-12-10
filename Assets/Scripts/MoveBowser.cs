using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBowser : MonoBehaviour {
    public float speed = 5f;
    public float height = 3f;
    private Vector3 pos;
    public float adjHeight = 10f;
	// Use this for initialization
	void Start () {
        pos = this.transform.position;
        Debug.Log(pos);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(0, Time.deltaTime * 20, 0);
        //get the objects current position and put it in a variable so we can access it later with less code
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, newY * height + adjHeight, pos.z) ;
    }
}
