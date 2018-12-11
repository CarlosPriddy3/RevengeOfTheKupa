using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : MonoBehaviour {
    public GameObject activeSavePoint;
    public Vector3 pos;
    public static SavePointManager instance = null;

    // Use this for initialization
    void Awake () {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
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
        return pos;
    }

    public void setPos(Vector3 pos)
    {
        this.pos = pos;
    }
}
