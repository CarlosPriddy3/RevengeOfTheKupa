using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSavePointHintLesson : MonoBehaviour {
    public Canvas savePointLessonCanvas;
    // Use this for initialization
    void Start()
    {
        savePointLessonCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update () {
     

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kupa")
        {
            savePointLessonCanvas.enabled = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kupa")
        {
            savePointLessonCanvas.enabled = false;
        }
    }

}
