using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour {
    public GameObject activeSavePoint;
    public Vector3 pos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setActiveSavePoint(GameObject savePoint)
    {
        this.activeSavePoint = savePoint;
        this.pos = savePoint.transform.position;
    }

    public Vector3 getSavePointPosition()
    {
        return this.activeSavePoint.transform.position;
    }
}
