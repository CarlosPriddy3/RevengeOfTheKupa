using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSavePoint : MonoBehaviour {
    private SavePointManager spManager;
    private Light spotlight;
    private Canvas checkPointCanvas;
	// Use this for initialization
	void Start () {
        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        GameObject checkpointCanvasObj = GameObject.FindGameObjectWithTag("CheckPointCanvas");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
        if (checkpointCanvasObj != null)
        {
            checkPointCanvas = checkpointCanvasObj.GetComponent<Canvas>();
        }
        spotlight = this.transform.parent.GetComponentInChildren<Light>();
        checkPointCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (spManager.getSavePointPosition() == this.transform.parent.position)
        { 
            spotlight.color = Color.green;
        } else
        {
            spotlight.color = Color.yellow;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kupa")
        {
            spManager.setActiveSavePoint(this.transform.parent.gameObject);
            checkPointCanvas.enabled = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kupa")
        {
            checkPointCanvas.enabled = false;
        }
    }
}
