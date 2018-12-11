using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    //Stamina
    private Slider staminaBar;
    private Image sliderFillImage;
    private Text staminaText;

    //Variables for jumping and landable surfaces
    public List<string> badSurfaceTags;
    private Vector3 frontRayDir;
    private Vector3 leftRayDir;
    private Vector3 rightRayDir;
    private Vector3 backRayDir;
    private RaycastHit belowHit;
    private RaycastHit directBelow;
    private RaycastHit leftBelow;
    private RaycastHit rightBelow;
    private RaycastHit forwardBelow;
    private RaycastHit backBelow;

    public Canvas kupaStartledCanvas;

	// Use this for initialization
	void Awake () {
        anim = this.gameObject.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();

        GameObject staminaBarObject = GameObject.FindGameObjectWithTag("StaminaBar");
        GameObject staminaFillBarObject = GameObject.FindGameObjectWithTag("StaminaFillBar");
        GameObject staminaTextObject = GameObject.FindGameObjectWithTag("StaminaText");
        if (staminaBarObject != null)
        {
            staminaBar = staminaBarObject.GetComponent<Slider>();
        }
        if (staminaFillBarObject != null)
        {
            sliderFillImage = staminaFillBarObject.GetComponent<Image>();
        }
        if (staminaTextObject != null)
        {
            staminaText = staminaTextObject.GetComponent<Text>();
        }
        
        
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
        
        //AddEvent(2, 0.05f, "playKupaJump", 0);
        soundTimer = 0f;
        forwardSpeedLimit = 1f;

        frontRayDir = (this.transform.up * -1).normalized;
        leftRayDir = (this.transform.up * -1).normalized;
        rightRayDir = (this.transform.up * -1).normalized;
        backRayDir = (this.transform.up * -1).normalized;
    }

    public Canvas getKupaStarledCanvas() {
        return kupaStartledCanvas;
    }
    private void Start()
    {
        prevPos = this.transform.position;
        //kupaStartledCanvas.enabled = false;
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
        velocityMag = new Vector3(velocity.x, 0, velocity.z).magnitude;

        float h = Input.GetAxisRaw("Horizontal");
        if (jumpTimer < 3f)
        {
            jumpTimer += Time.deltaTime;
        }
        
        switch (kupaState)
        {
            case KupaState.NotSpinning:
                if (staminaBar.value < 100)
                {
                    staminaBar.value += Time.deltaTime * 5;
                    staminaText.text = "Stamina: " + ((int)staminaBar.value).ToString();
                }
                
                isSpinning = false;

                this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f; // default friction;
                this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = 0f; // default bounciness;
                
                float v = Input.GetAxisRaw("Vertical");
                Move(v);
                Turn(h, turnScalar);
                Animating(v);
                if (walking)
                {
                    anim.applyRootMotion = false;
                }
                /*bool groundClose = Physics.Raycast(transform.position, down, distToGround);
                isGrounded = Physics.Raycast(transform.position, down, distToGroundForGrounded);
                anim.SetBool("groundClose", groundClose);
                anim.SetBool("isGrounded", isGrounded);*/
                
                if (canJump())
                {
                    playKupaJump();
                    Jump();
                    jumpTimer = 0f;
                }
                
                if ((staminaBar.value >= 20) && isGrounded && (Input.GetKey(KeyCode.LeftShift) || isGrounded && Input.GetButtonDown("Fire3")))
                {
                    kupaState = KupaState.Spinning;
                    anim.Play("DropIntoShell");
                    playSlowSpinSound();
                    anim.SetBool("isSpinning", true);
                    hasShot = false;
                    spinPowerTimer = 0f;
                    kupaArrow.SetActive(true);
                } else if (!isGrounded)
                {
                    //No Error message needed
                } else if (staminaBar.value < 20)
                {
                    //Let player know stamina is too low
                }
                break;
            case KupaState.Spinning:
                

                Color lerpedColor = Color.Lerp(defaultArrowColor, Color.red, spinPowerTimer / 3f);
                kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", lerpedColor);
                kupaArrow.GetComponent<Renderer>().material.SetColor("_EmissionColor", lerpedColor);
                soundTimer += Time.deltaTime;
                if (soundTimer > .778f)
                {
                    playSpinSound();
                    soundTimer = 0;
                }
                //Limit Spin Power
                if (spinPowerTimer <= 3f)
                {
                    spinPowerTimer += Time.deltaTime;
                }

                //Ability to turn in shell form
                Turn(h, turnScalar / 2);
                shooting = velocity.magnitude > stopSpinVel;
                if (!hasShot)
                {
                    velocityMag = 0f;
                }
                if ((!hasShot && !shooting && isGrounded && (Input.GetButton("Fire3"))))
                {
                    kupaArrow.SetActive(true);
                }
                    if (!hasShot && !shooting && isGrounded && !(Input.GetButton("Fire3")))
                {
                    //Shoot Forward
                    staminaBar.value -= 20f;
                    staminaText.text = "Stamina: " + ((int)staminaBar.value).ToString();
                    this.gameObject.GetComponent<CapsuleCollider>().material.dynamicFriction = dynFric;
                    this.gameObject.GetComponent<CapsuleCollider>().material.bounciness = shellBounce;
                    if (spinPowerTimer < 1f)
                    {
                        spinPowerTimer = 1f;
                    }
                    rb.AddForce(this.gameObject.transform.forward * shootStrength * spinPowerTimer * 1.75f);
                    hasShot = true;
                    shooting = true;
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    isSpinning = true;
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_EmissionColor", defaultArrowColor);
                }
                if (!shooting && hasShot)
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_EmissionColor", defaultArrowColor);
                    stopSound();
                }
                if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
                {
                    anim.Play("Idle");
                    kupaState = KupaState.NotSpinning;
                    anim.SetBool("isSpinning", false);
                    kupaArrow.SetActive(false);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_Color", defaultArrowColor);
                    kupaArrow.GetComponent<Renderer>().material.SetColor("_EmissionColor", defaultArrowColor);
                    stopSound();
                }
                break;
        }
    }

    void Jump()
    {
        staminaBar.value -= 10f;
        anim.Play("Launch");
        anim.applyRootMotion = false;
        Debug.DrawRay(this.transform.position, velocity * 10, Color.red);
        float velX = velocity.x;
        float velZ = velocity.z;
        Vector3 xzVel = new Vector3(velX, 0, velZ);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce((xzVel * 10) + new Vector3(0, JumpHeight, 0));
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
        if (v > 0)
        {
            anim.SetFloat("velz", forward);
        }
        
        this.transform.Translate(Vector3.forward * velz * moveScalar * airRes  * forwardSpeedLimit * Time.deltaTime);
        
        
    }

    void Turn(float h, float scalar)
    {
        this.transform.Rotate(new Vector3(0f, h, 0f) * scalar * Time.deltaTime);
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
        kupaJump.Stop();
        kupaJump.Play();
    }
    void playSpinSound()
    {
        slowestSpinSound.Stop();
        slowSpinSound.Stop();
        spinSound.Stop();
        spinSound.Play();
    }
    void playSlowSpinSound()
    {
        slowSpinSound.Stop();
        slowSpinSound.Play();
    }
    void playSlowestSpinSound()
    {
        slowestSpinSound.Stop();
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

    bool canJump()
    {    
        if (staminaBar.value < 10)
        {
            return false;
        }
        Debug.DrawRay(this.transform.position + this.transform.forward * 1.3f  + this.transform.up * -2.35f, down, Color.red);
        Debug.DrawRay(this.transform.position + this.transform.forward * 0.65f + this.transform.right * 0.65f + this.transform.up * -2.35f, down, Color.red);
        Debug.DrawRay(this.transform.position + this.transform.forward * 0.65f + this.transform.right * -0.65f + this.transform.up * -2.35f, down, Color.red);
        Debug.DrawRay(this.transform.position + this.transform.up * -2.35f, down, Color.red);
        Vector3 frontGroundRay = (this.transform.position + this.transform.forward * 1.3f + this.transform.up * -2.35f);
        Vector3 rightGroundRay = (this.transform.position + this.transform.forward * 0.65f + this.transform.right * 0.65f + this.transform.up * -2.35f);
        Vector3 leftGroundRay = (this.transform.position + this.transform.forward * 0.65f + this.transform.right * -0.65f + this.transform.up * -2.35f);
        Vector3 backGroundRay = (this.transform.position + this.transform.forward * 0f + this.transform.up * -2.35f);

        bool landableDirectBelow = (Physics.Raycast(transform.position, down, out directBelow, distToGroundForGrounded) && directBelow.collider != null && !badSurfaceTags.Contains(directBelow.collider.tag));
        bool landableLeftBelow = (Physics.Raycast(leftGroundRay, down, out leftBelow, 1f) && leftBelow.collider != null && !badSurfaceTags.Contains(leftBelow.collider.tag));
        bool landableRightBelow = (Physics.Raycast(rightGroundRay, down, out rightBelow, 1f) && rightBelow.collider != null && !badSurfaceTags.Contains(rightBelow.collider.tag));
        bool landableForwardBelow = (Physics.Raycast(frontGroundRay, down, out forwardBelow, 1f) && forwardBelow.collider != null && !badSurfaceTags.Contains(forwardBelow.collider.tag));
        bool landableBackBelow = (Physics.Raycast(backGroundRay, down, out backBelow, 1f) && backBelow.collider != null && !badSurfaceTags.Contains(backBelow.collider.tag));
        isGrounded = (landableDirectBelow || landableLeftBelow|| landableRightBelow || landableForwardBelow || landableBackBelow);
        bool groundClose = (Physics.Raycast(transform.position, down, out belowHit, distToGround));
        //Collider Check Debugs
        /*if (forwardBelow.collider != null)
        {
            Debug.Log("NAME: " + forwardBelow.collider.tag);
            if (forwardBelow.collider.transform.parent != null)
            {
                Debug.Log("PARENT NAME: " + forwardBelow.transform.parent.tag);
            }
            
        }*/
        /*bool onObject = (Physics.Raycast(transform.position, down, distToGroundForGrounded)
            || Physics.Raycast(this.transform.position + this.transform.forward * 1.3f + this.transform.up * -2.35f, down, out forwardBelow, 1f)
            || Physics.Raycast(this.transform.position + this.transform.forward * 0.65f + this.transform.right * 0.65f + this.transform.up * -2.35f, down, out rightBelow, 1f)
            || Physics.Raycast(this.transform.position + this.transform.forward * 0.65f + this.transform.right * -0.65f + this.transform.up * -2.35f, down, 1f)
            || Physics.Raycast(this.transform.position + this.transform.forward * 0f + this.transform.up * -2.35f, down, 1f));*/

        anim.SetBool("groundClose", groundClose);
        anim.SetBool("landableObject", isGrounded);
        anim.SetBool("isGrounded", isGrounded);
        if (isGrounded && (Input.GetKey(KeyCode.Space) || isGrounded && Input.GetButtonDown("Fire1")))
        {
            if (jumpTimer > jumpCooldown)
            {
                return true;
            }

        }
        return false;
    }
}
