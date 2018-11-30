using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStealthLesson : MonoBehaviour {
    public Canvas stealthLessonCanvas;
    bool hasBeenTriggered;
	// Use this for initialization
	void Start () {
        hasBeenTriggered = false;
        stealthLessonCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Kupa" && !hasBeenTriggered)
        {
            stealthLessonCanvas.enabled = true;
            hasBeenTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kupa")
        {
            stealthLessonCanvas.enabled = false;
        }
    }
}
