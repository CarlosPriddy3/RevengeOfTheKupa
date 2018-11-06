using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveScalar = 20f;
    public float turnScalar = 150f;
    public float distToGround = 7.5f;
    public float distToGroundForGrounded = 3.605f;
    public float JumpHeight = 1700;
    public float GravityStrength = -60f;
    public float shootStrength = 1000f;
    public float dynFric = 0.1f;
    public float shellBounce = 1f;
    public float stopSpinVel = 2f;
    public bool isGrounded;
    public bool isSpinning;
    public bool shooting;
    public bool hasShot;
    public float spinPowerTimer;
    private float jumpCooldown = 1f;
    private float jumpTimer;
    private bool walking;
    private Vector3 prevPos;
    public Vector3 velocity;
    public float velocityMag;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 down;
    private GameObject kupaArrow;
    public KupaState kupaState;
    private Color defaultArrowColor;
    
    
	// Use this for initialization
	void Awake () {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        
        
        Vector3 gravityS = new Vector3(0, GravityStrength, 0);
        Physics.gravity = gravityS;
        jumpTimer = 0f;
        anim.SetBool("isGrounded", true);
        down = this.transform.up * -1;
        walking = false;
        isGrounded = true;
        isSpinning = false;
        kupaState = KupaState.NotSpinning;
        shooting = false;
        spinPowerTimer = 0f;

        //Arrow
        
        kupaArrow = GameObject.FindGameObjectWithTag("KupaArrow");
        kupaArrow.SetActive(false);
        defaultArrowColor =  kupaArrow.GetComponent<Renderer>().material.GetColor("_Color");
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
        float h = Input.GetAxisRaw("Horizontal");
        if (jumpTimer < 3f)
        {
            jumpTimer += Time.deltaTime;
        }
        
        switch (kupaState)
        {
            case KupaState.NotSpinning:
                this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f; // default friction;
                this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = 0f; // default bounciness;
                
                float v = Input.GetAxisRaw("Vertical");
                Move(v);
                Turn(h);
                Animating(v);
                if (walking)
                {
                    anim.applyRootMotion = false;
                }
                /*bool groundClose = Physics.Raycast(transform.position, down, distToGround);
                isGrounded = Physics.Raycast(transform.position, down, distToGroundForGrounded);
                anim.SetBool("groundClose", groundClose);
                anim.SetBool("isGrounded", isGrounded);*/
                RaycastHit belowHit;
                bool landableObject = true;
                bool groundClose = Physics.Raycast(transform.position, down, out belowHit, distToGround);
                
                bool onObject = Physics.Raycast(transform.position, down, distToGroundForGrounded);
                if (belowHit.collider != null)
                {
                    Debug.Log(belowHit.collider.gameObject.tag);
                    landableObject = belowHit.collider.gameObject.tag != "MarioCollider";

                } 
                
                isGrounded = onObject && landableObject;
                anim.SetBool("groundClose", groundClose);
                anim.SetBool("landableObject", landableObject);
                anim.SetBool("isGrounded", isGrounded);
                if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
                {
                    if (jumpTimer > jumpCooldown)
                    {
                        Jump();
                        jumpTimer = 0f;
                    }
                    
                }
                if (isGrounded && (Input.GetKey(KeyCode.LeftShift) || isGrounded && Input.GetButtonDown("Fire3")))
                {
                    kupaState = KupaState.Spinning;
                    anim.Play("DropIntoShell");
                    anim.SetBool("isSpinning", true);
                    hasShot = false;
                    spinPowerTimer = 0f;
                    kupaArrow.SetActive(true);
                }
                break;
            case KupaState.Spinning:
                Color lerpedColor = Color.Lerp(defaultArrowColor, Color.red, spinPowerTimer / 5f);
                kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", lerpedColor);

                //Limit Spin Power
                if (spinPowerTimer <= 5f)
                {
                    spinPowerTimer += Time.deltaTime;
                }

                //Ability to turn in shell form
                Turn(h);
                shooting = velocity.magnitude > stopSpinVel;
                Debug.Log("SHOOTING " + shooting);
                if ((!hasShot && !shooting && isGrounded && (Input.GetButton("Fire3"))))
                {
                    kupaArrow.SetActive(true);
                }
                    if ((!hasShot && !shooting && isGrounded && !(Input.GetButton("Fire3"))))
                {
                    //Shoot Forward
                    Debug.Log("SHOOT");
                    this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = dynFric;
                    this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = shellBounce;
                    rb.AddForce(this.gameObject.transform.forward * shootStrength * spinPowerTimer);
                    hasShot = true;
                    shooting = true;
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                }
                if (!shooting && hasShot)
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                }
                if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                }
                break;
        }
    }

    void Jump()
    {
        anim.Play("Launch");
        anim.applyRootMotion = false;
        Debug.DrawRay(this.transform.position, velocity * 10, Color.red);
        rb.AddForce((velocity * 10) + new Vector3(0, JumpHeight, 0));
        isGrounded = false;
        anim.SetBool("isGrounded", false);
    }

    void Move(float v)
    {
        float velz = Input.GetAxis("Vertical");
        anim.SetFloat("velz", velz);
        float airRes = 1f;
        if (!isGrounded)
        {
            airRes = 0.5f;
        }
        if (v > 0.5f)
        {
            this.transform.Translate(Vector3.forward * v * moveScalar * airRes * Time.deltaTime);
        } else if (v < -0.5f)
        {
            this.transform.Translate(Vector3.forward * v * (moveScalar * .1f) * Time.deltaTime);
        }
        
        
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
