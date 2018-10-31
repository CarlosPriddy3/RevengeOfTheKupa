using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveScalar = 20f;
    public float turnScalar = 150f;
    public float distToGround = 7.5f;
    public float distToGroundForGrounded = 3.605f;
    public float JumpHeight = 3;
    public float GravityStrength = -60f;
    public float shootStrength = 2000f;
    public float dynFric = 0.1f;
    public float shellBounce = 3f;
    public float stopSpinVel = 2f;
    public bool isGrounded;
    public bool isSpinning;
    public bool shooting;
    public bool hasShot;
    private bool walking;
    private Vector3 prevPos;
    public Vector3 velocity;
    public float velocityMag;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 down;
    private KupaState kupaState;
    
    
	// Use this for initialization
	void Awake () {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        
        Vector3 gravityS = new Vector3(0, GravityStrength, 0);
        Physics.gravity = gravityS;
        anim.SetBool("isGrounded", true);
        down = this.transform.up * -1;
        walking = false;
        isGrounded = true;
        isSpinning = false;
        kupaState = KupaState.NotSpinning;
        shooting = false;
    }
    private void Start()
    {
        prevPos = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = (this.transform.position - prevPos) / Time.deltaTime;
        prevPos = this.transform.position;
        velocityMag = velocity.magnitude;
        switch (kupaState)
        {
            case KupaState.NotSpinning:
                this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f; // default friction;
                this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = 0f; // default bounciness;
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Move(v);
                Turn(h);
                Animating(v);
                if (walking)
                {
                    anim.applyRootMotion = false;
                }
                bool groundClose = Physics.Raycast(transform.position, down, distToGround);
                isGrounded = Physics.Raycast(transform.position, down, distToGroundForGrounded);
                anim.SetBool("groundClose", groundClose);
                anim.SetBool("isGrounded", isGrounded);
                if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
                {
                    Jump();
                }
                if (isGrounded && (Input.GetKey(KeyCode.LeftShift) || isGrounded && Input.GetButtonDown("Fire3")))
                {
                    kupaState = KupaState.Spinning;
                    anim.Play("DropIntoShell");
                    anim.SetBool("isSpinning", true);
                    hasShot = false;
                }
                break;
            case KupaState.Spinning:
                shooting = velocity.magnitude > stopSpinVel;
                Debug.Log("SHOOTING " + shooting);
                if ((!hasShot && !shooting && isGrounded && !(Input.GetButton("Fire3"))))
                {
                    //Shoot Forward
                    Debug.Log("SHOOT");
                    this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = dynFric;
                    this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = shellBounce;
                    rb.AddForce(this.gameObject.transform.forward * shootStrength);
                    hasShot = true;
                    shooting = true;
                }
                if (!shooting && hasShot)
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                }
                break;
        }
        

        /* else
        {
            anim.applyRootMotion = true;
        }*/
    }

    void Jump()
    {
        anim.Play("Launch");
        anim.applyRootMotion = false;
        rb.AddForce(0, JumpHeight, 0);
        isGrounded = false;
        anim.SetBool("isGrounded", false);
    }

    void Move(float v)
    {
        //movement.Set(0f, 0f, v);
        //movement = movement.normalized * moveScalar * Time.deltaTime;
        //koopaRigidBody.MovePosition(transform.position + movement);
        this.transform.Translate(Vector3.forward * v * moveScalar * Time.deltaTime);
        float velz = Input.GetAxis("Vertical");
        anim.SetFloat("velz", velz);
    }

    void Turn(float h)
    {
        this.transform.Rotate(new Vector3(0f, h, 0f) * turnScalar * Time.deltaTime);
        foreach (Transform childTransform in this.gameObject.GetComponentsInChildren<Transform>())
        {
            transform.rotation = this.transform.rotation;
        }
    }

    void Animating(float v)
    {
        walking = (v != 0f);
        if (isGrounded)
        {
            anim.SetBool("isWalking", walking);
        }   
    }

   /* void OnAnimatorMove()
    {
        Debug.Log("IN ONANIMATORMOVE");
        Vector3 newRootPosition;
        Quaternion newRootRotation;

        if (this.gameObject.GetComponent<JumpListener>().isGrounded)
        {
            //use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            newRootPosition = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;

        //TODO Here, you could scale the difference in position and rotation to make the character go faster or slower
        this.transform.position = Vector3.LerpUnclamped(this.transform.position, newRootPosition, moveScalar);
        this.transform.rotation = Quaternion.LerpUnclamped(this.transform.rotation, newRootRotation, turnScalar);
        //this.transform.position = newRootPosition;
        //this.transform.rotation = newRootRotation;
    }*/
}
