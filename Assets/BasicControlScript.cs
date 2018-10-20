using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class BasicControlScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    public float forwardMaxSpeed = 15f;
    public float turnMaxSpeed = 100f;

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

    public bool isGrounded;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        cinput = GetComponent<CharacterInputController>();
    }


    void Start()
    {
        isGrounded = false;
        rbody.sleepThreshold = 0f;
    }

    void FixedUpdate()
    {
        float inputForward = 0f;
        float inputTurn = 0f;

        if (cinput.enabled)
        {
            inputForward = cinput.Forward;
            inputTurn = cinput.Turn;
        }

        if (inputForward < 0f)
            inputTurn = -inputTurn;
        
        rbody.MovePosition(rbody.position + this.transform.forward * inputForward * Time.deltaTime * forwardMaxSpeed);
        rbody.MoveRotation(rbody.rotation * Quaternion.AngleAxis(inputTurn * Time.deltaTime * turnMaxSpeed, Vector3.up));

        anim.SetFloat("vely", inputForward);
        anim.SetBool("isFalling", !isGrounded);
    }
}
