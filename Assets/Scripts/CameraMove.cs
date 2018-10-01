using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public GameObject camera;
    Transform cameraT;

    public int Boundary = 50;
    public int speed = 5;
    public int rotSpeed = 30;
 
    private int theScreenWidth;
	// Use this for initialization
	void Start () {
        theScreenWidth = Screen.width;
        cameraT = camera.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            // Move to the right
            cameraT.position = cameraT.position + Camera.main.transform.right * speed * Time.deltaTime;
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            // Move to the left
            cameraT.position = cameraT.position + Camera.main.transform.right * -1f * speed * Time.deltaTime;
        }
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            // Move to the forward
            cameraT.position = cameraT.position + Camera.main.transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            // Move to the backward
            cameraT.position = cameraT.position + Camera.main.transform.forward  * -1f * speed * Time.deltaTime;
        }
        if (Input.mousePosition.x > theScreenWidth - Boundary)
        {
            cameraT.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }

        if (Input.mousePosition.x < 0 + Boundary)
        {
            cameraT.Rotate(0, -1f * rotSpeed * Time.deltaTime, 0);
        }

    }
}
