using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallListener : MonoBehaviour {
    public GameEndAction action;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kupa")
        {
            action.onLoss();
        }
    }
}
