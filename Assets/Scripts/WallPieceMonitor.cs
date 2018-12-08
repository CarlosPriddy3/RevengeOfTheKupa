using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPieceMonitor : MonoBehaviour {
    GameObject[] brokenWalls;
    public float timeOut;
    public float startDisappear;
    public Material rusty1;
    private Color defaultColor;
	// Use this for initialization
	void Start () {
        brokenWalls = GameObject.FindGameObjectsWithTag("BreakableWall");
        defaultColor = rusty1.color;
	}
	
	// Update is called once per frame
	void Update () {
        brokenWalls = GameObject.FindGameObjectsWithTag("BreakableWall");
        foreach (GameObject brokenWall in brokenWalls)
        {
            TimeManagerForWalls timeManager = brokenWall.GetComponent<TimeManagerForWalls>();
            if (timeManager.time > startDisappear)
            {
                foreach (Transform wallPiece in brokenWall.transform)
                {
                    Color color = Color.Lerp(defaultColor, new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0), timeManager.time / timeOut);
                    wallPiece.gameObject.GetComponent<Renderer>().material.color = color;
                }
            }
            
            if (timeManager.time > (timeOut * 2 / 3))
            {    
                Destroy(brokenWall);
            }
        }
    }
}
