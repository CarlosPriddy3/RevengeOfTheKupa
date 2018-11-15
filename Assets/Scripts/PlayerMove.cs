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
    private float forwardSpeedLimit;
    public AudioSource leftFootStep;
    public AudioSource rightFootStep;
    public AudioSource kupaJump;
    public AudioSource spinSound;
    public AudioSource slowSpinSound;
    public AudioSource slowestSpinSound;
    public AudioSource shellTakeoffSound;
    private float soundTimer;
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
        
        AddEvent(3, 0.13f, "playKupaJump", 0);
        soundTimer = 0f;
        forwardSpeedLimit = 0.5f;
    }
    private void Start()
    {
        prevPos = this.transform.position;
    }

    public bool getSpinning()
    {
        return isSpinning;
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
                isSpinning = false;

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
                    playSlowSpinSound();
                    anim.SetBool("isSpinning", true);
                    hasShot = false;
                    spinPowerTimer = 0f;
                    kupaArrow.SetActive(true);
                }
                break;
            case KupaState.Spinning:
                

                Color lerpedColor = Color.Lerp(defaultArrowColor, Color.red, spinPowerTimer / 5f);
                kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", lerpedColor);
                soundTimer += Time.deltaTime;
                if (soundTimer > .778f)
                {
                    playSpinSound();
                    soundTimer = 0;
                }
                //Limit Spin Power
                if (spinPowerTimer <= 5f)
                {
                    spinPowerTimer += Time.deltaTime;
                }

                //Ability to turn in shell form
                Turn(h);
                shooting = velocity.magnitude > stopSpinVel;
                if ((!hasShot && !shooting && isGrounded && (Input.GetButton("Fire3"))))
                {
                    kupaArrow.SetActive(true);
                }
                    if ((!hasShot && !shooting && isGrounded && !(Input.GetButton("Fire3"))))
                {
                    //Shoot Forward
                    playShellTakeoffSound();
                    this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = dynFric;
                    this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = shellBounce;
                    rb.AddForce(this.gameObject.transform.forward * shootStrength * spinPowerTimer);
                    hasShot = true;
                    shooting = true;
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    isSpinning = true;
                }
                if (!shooting && hasShot)
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    stopSound();
                }
                if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    stopSound();
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
        
        if (Input.GetKeyUp(KeyCode.Alpha1))
            forwardSpeedLimit = 0.1f;
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            forwardSpeedLimit = 0.2f;
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            forwardSpeedLimit = 0.3f;
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            forwardSpeedLimit = 0.4f;
        else if (Input.GetKeyUp(KeyCode.Alpha5))
            forwardSpeedLimit = 0.5f;
        else if (Input.GetKeyUp(KeyCode.Alpha6))
            forwardSpeedLimit = 0.6f;
        else if (Input.GetKeyUp(KeyCode.Alpha7))
            forwardSpeedLimit = 0.7f;
        else if (Input.GetKeyUp(KeyCode.Alpha8))
            forwardSpeedLimit = 0.8f;
        else if (Input.GetKeyUp(KeyCode.Alpha9))
            forwardSpeedLimit = 0.9f;
        else if (Input.GetKeyUp(KeyCode.Alpha0))
            forwardSpeedLimit = 1.0f;
        if (!isGrounded)
        {
            airRes = 0.5f;
        }
        float forward = velz * moveScalar * airRes * forwardSpeedLimit * Time.deltaTime * 2;
        anim.SetFloat("velz", forward);
        if (v > 0.5f)
        {
            this.transform.Translate(Vector3.forward * velz * moveScalar * airRes  * forwardSpeedLimit * Time.deltaTime);
        } else if (v < -0.5f)
        {
            this.transform.Translate(Vector3.forward * velz * forwardSpeedLimit * (moveScalar * .1f) * Time.deltaTime);
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
    void stopSound()
    {
        spinSound.Stop();
        slowSpinSound.Stop();
        slowestSpinSound.Stop();
    }
    void playLeftFootStep()
    {
        leftFootStep.Play();
    }
    void playRightFootStep()
    {
        rightFootStep.Play();
    }
    void playKupaJump()
    {
        Debug.Log("IN ANIMATION EVENT");
        kupaJump.Play();
    }
    void playSpinSound()
    {
        spinSound.Play();
    }
    void playSlowSpinSound()
    {
        slowSpinSound.Play();
    }
    void playSlowestSpinSound()
    {
        slowestSpinSound.Play();
    }
    void playShellTakeoffSound()
    {
        shellTakeoffSound.Play();
    }

    //Used to add animation events to this animator
    void AddEvent(int Clip, float time, string functionName, float floatParameter)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.floatParameter = floatParameter;
        animationEvent.time = time;
        AnimationClip clip = anim.runtimeAnimatorController.animationClips[Clip];
        clip.AddEvent(animationEvent);
    }
}
