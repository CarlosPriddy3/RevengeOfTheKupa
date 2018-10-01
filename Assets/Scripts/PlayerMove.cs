using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveScalar = 1f;
    public float turnScalar = 20f;
    Animator anim;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(v);
        Turn(h);
        Animating(v);
    }

    void Move(float v)
    {
        //movement.Set(0f, 0f, v);
        //movement = movement.normalized * moveScalar * Time.deltaTime;
        //koopaRigidBody.MovePosition(transform.position + movement);
        this.transform.Translate(Vector3.forward * v * moveScalar * Time.deltaTime);
        float velz = Input.GetAxis("Vertical");
        Debug.Log(velz);
        anim.SetFloat("velz", velz);
    }

    void Turn(float h)
    {
        this.transform.Rotate(new Vector3(0f, h, 0f) * turnScalar * Time.deltaTime);
    }

    void Animating(float v)
    {
        bool walking = (v != 0f);
        Debug.Log("Walking " + walking);
        anim.SetBool("isWalking", walking);
    }
}
