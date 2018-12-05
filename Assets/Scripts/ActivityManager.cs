using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour {
    private List<GameObject> Marios;
    private Vector3 kupaPos;
    public float maxEnableDis = 75f;
	// Use this for initialization
	void Start () {
        Marios = new List<GameObject>();
        foreach (GameObject mario in GameObject.FindGameObjectsWithTag("Mario"))
        {
            Marios.Add(mario);
        }
        foreach (GameObject firemario in GameObject.FindGameObjectsWithTag("FireMario"))
        {
            Marios.Add(firemario);
        }
        foreach (GameObject stationarymario in GameObject.FindGameObjectsWithTag("StationaryFireMario"))
        {
            Marios.Add(stationarymario);
        }

        kupaPos = GameObject.FindGameObjectWithTag("Kupa").transform.position;

	}
	
	// Update is called once per frame
	void Update () {
        kupaPos = GameObject.FindGameObjectWithTag("Kupa").transform.position;
        foreach (GameObject mario in Marios)
        {
            float disToTarget = Vector3.Magnitude(kupaPos - mario.transform.position);
            if (disToTarget > maxEnableDis && mario.activeInHierarchy == true)
            {
                mario.SetActive(false);
            } else if (disToTarget <= maxEnableDis && mario.activeInHierarchy == false)
            {
                mario.SetActive(true);
            }
        }
	}
}
