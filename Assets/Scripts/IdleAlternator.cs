using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAlternator : MonoBehaviour {
    public Animator anim;
    private float timer;
    public float timeBetweenActions = 10f;
	// Use this for initialization
	void Start () {
        anim.SetBool("IdleAction", false);
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= timeBetweenActions && !anim.GetBool("IdleAction"))
        {
            anim.SetBool("IdleAction", true);
            timer = 0f;
        } else if (timer >= timeBetweenActions)
        {
            anim.SetBool("IdleAction", false);
            timer = 0f;
        }
	}
}
