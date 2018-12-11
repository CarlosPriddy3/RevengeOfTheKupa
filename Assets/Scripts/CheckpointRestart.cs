using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Class to encapsulate checkpoint restart action.
/// </summary>
public class CheckpointRestart : MonoBehaviour
{
    SavePointManager spManager;
    bool hasCheckpoint;
    // Use this for initialization
    void Start()
    {

        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
        hasCheckpoint = true;
        if (spManager != null)
        {
            //No Checkpoint Reached! Grey out option!
            if (spManager.getSavePointPosition() == Vector3.zero)
            {
                hasCheckpoint = false;
            }
        }
    }

    /// <summary>
    /// Restarts from the checkpoint.
    /// </summary>
    public void restart()
	{
        if (hasCheckpoint)
        {
            SceneManager.LoadScene("Level01Remake");
            Debug.Log("Restarted from checkpoint");
        }
        
	}
}
