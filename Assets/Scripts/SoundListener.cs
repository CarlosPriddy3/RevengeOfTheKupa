using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoundListener : MonoBehaviour {
    public bool smallSound = false;
    public bool bigSound = false;
    public float smallSpeedThreshold = 4f;
    public float bigSpeedThreshold = 10f;
    private GameObject kupaTrupa;
    private PlayerMove kupaControl;
    private bool regularMario;
    private MarioController marioController;
    private GameObject[] investigationPoints;

	// Use this for initialization
	void Start () {
        kupaTrupa = GameObject.FindGameObjectWithTag("Kupa");
        kupaControl = kupaTrupa.GetComponent<PlayerMove>();
        marioController = this.transform.parent.GetComponent<MarioController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (marioController.aiState == MarioController.AIState.Patrol)
        {
            if (other.name == "SupaKupaTrupa")
            {
                Debug.Log(" SMALLSOUND " + smallSound + " BIGSOUND " + bigSound);
                Debug.Log(kupaControl.velocityMag);
                if (smallSound)
                {
                    if (kupaControl.velocityMag > smallSpeedThreshold)
                    {
                        Debug.Log("INVESTIGATE ");
                        marioController.InstantiateInvestigateParams(kupaTrupa.transform.position);
                    }
                }
                else if (bigSound)
                {
                    if (kupaControl.velocityMag > bigSpeedThreshold)
                    {
                        Debug.Log("INVESTIGATE ");
                        marioController.InstantiateInvestigateParams(kupaTrupa.transform.position);
                    }
                }
            }
        }
        Debug.Log(other.name == "SupaKupaTrupa");
        
        
    }
}
