using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSavePoint : MonoBehaviour {
    private SavePointManager spManager;
	// Use this for initialization
	void Start () {
        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kupa")
        {
            spManager.setActiveSavePoint(this.transform.parent.gameObject);
        }
    }
}
