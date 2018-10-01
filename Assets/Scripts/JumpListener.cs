using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpListener : MonoBehaviour {
    public Animator anim;
    private RaycastHit hit;
    public float distToGround = 5f;
    private Vector3 dir;
    private bool isGrounded;

	// Use this for initialization
	void Start () {
        anim.SetBool("Jump", false);
        dir = this.transform.up * -1;
        isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Jump", false);
        Debug.DrawRay(transform.position, dir * distToGround, Color.red);
        if (Physics.Raycast(transform.position, dir, distToGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (isGrounded && (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Fire1")))
        {
            Debug.Log("In Jump");
            anim.SetBool("Jump", true);
            isGrounded = false;
        }
    }
}
