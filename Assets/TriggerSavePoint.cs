using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSavePoint : MonoBehaviour {
    private SavePointManager spManager;
    private Light spotlight;
	// Use this for initialization
	void Start () {
        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
        spotlight = this.transform.parent.GetComponentInChildren<Light>();
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
        }
    }
}
