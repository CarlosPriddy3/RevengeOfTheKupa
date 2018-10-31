using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpListener : MonoBehaviour {
    public Animator anim;
    private RaycastHit hit;
    public float distToGround = 5f;
    public float distToGroundForGrounded = 3f;
    private Vector3 dir;
    public bool isGrounded;
    public bool isSpinning;
    private Rigidbody rb;
    public float JumpHeight = 800;
    public float GravityStrength = -45;

	// Use this for initialization
	void Start () {
        anim.SetBool("isGrounded", true);
        dir = this.transform.up * -1;
        isGrounded = true;
        isSpinning = false;
        rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawRay(transform.position, dir * distToGround, Color.blue);
        Debug.DrawRay(transform.position, dir * distToGroundForGrounded, Color.red);
        isSpinning = (anim.GetCurrentAnimatorStateInfo(0).IsName("DropIntoShell") || anim.GetCurrentAnimatorStateInfo(0).IsName("ShellSpin"));
        bool groundClose = Physics.Raycast(transform.position, dir, distToGround);
        anim.SetBool("groundClose", groundClose);
        isGrounded = Physics.Raycast(transform.position, dir, distToGroundForGrounded);
        anim.SetBool("isGrounded", isGrounded);
        /*
        else
        {
            isGrounded = false;
        }*/
        if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
        {
            /* Debug.Log("In Jump");
             anim.SetBool("Jump", true);
             isGrounded = false;
             anim.applyRootMotion = false;
             rb.AddForce(new Vector3(0, 400, 0));*/
            Jump();
        }
        else if (!isSpinning && isGrounded && (Input.GetKey(KeyCode.LeftShift)))
        {
            anim.Play("DropIntoShell");
            anim.SetBool("isSpinning", true);
            isSpinning = true;
            anim.applyRootMotion = false;
        }
    }

    private void Jump()
    {
        anim.Play("Launch");
        anim.applyRootMotion = false;
        rb.AddForce(0, JumpHeight, 0);
        isGrounded = false;
        anim.SetBool("isGrounded", false);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            anim.applyRootMotion = true;
            isGrounded = true;
            anim.SetBool("isGrounded", true);
        }
    }*/
}
