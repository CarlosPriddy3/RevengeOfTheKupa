using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    public float timeBetweenClicks;

    float clickCooldown;
    bool somethingClicked;
	// Use this for initialization
	void Start () {
        clickCooldown = 100f;
	}
	
	// Update is called once per frame
	void Update () {
        clickCooldown += Time.deltaTime;
        if (Input.GetButton("Fire1") && (clickCooldown > timeBetweenClicks) && !somethingClicked)
        {
            CastRay();
            clickCooldown = 0;
        }
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }
        
    }

    void CastRay ()
    {
        Debug.Log("InCastRay");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            hit.transform.SendMessage("HitByRay");
            somethingClicked = true;
        }
        return;
    }
}
