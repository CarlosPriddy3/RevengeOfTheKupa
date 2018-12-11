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

    // Use this for initialization
    void Start()
    {

        GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
    }

    /// <summary>
    /// Restarts from the checkpoint.
    /// </summary>
    public void restart()
	{
        SceneManager.LoadScene("Level01Remake");
	}
}
