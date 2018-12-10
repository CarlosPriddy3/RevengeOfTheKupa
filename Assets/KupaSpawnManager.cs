using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KupaSpawnManager : MonoBehaviour {
    private SavePointManager spManager;
    public GameObject defaultKupaSpawn;
	// Use this for initialization
	void Start () {
        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        GameObject kupa = GameObject.FindGameObjectWithTag("Kupa");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
        if (spManager != null)
        {
            if (spManager.getSavePointPosition() != Vector3.zero)
            {
                if (kupa != null)
                {
                    Vector3 pos = spManager.getSavePointPosition();
                    float x = pos.x;
                    float z = pos.z;
                    kupa.transform.position = new Vector3(x, 1, z);
                }
            }
            else
            {
                kupa.transform.position = defaultKupaSpawn.transform.position;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
