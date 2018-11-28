using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    public List<Tutorial> Tutorials = new List<Tutorial>();

    public Text expText;

    private static TutorialManager instance;

    public static TutorialManager Instance {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TutorialManager>();
            if (instance == null)
                Debug.Log("There is no TutorialManager");

            return instance;
        }
    }

    private Tutorial currentTutorial;

	// Use this for initialization
	void Start () {
        SetNextTutorial(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (currentTutorial) {
            currentTutorial.CheckIfHappening();
        }
	}

    public void CompletedTutorial() {
        SetNextTutorial(currentTutorial.Order + 1);
    }

    public void SetNextTutorial(int currentOrder) {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial) {
            CompletedAllTutorials();
            return;
        }

        expText.text = currentTutorial.Explanation;
    }

    public void CompletedAllTutorials() {
        expText.text = "You have completed all the tutorials";
        SceneManager.LoadSceneAsync("Level01");
        GameState.state = State.PLAY;
    }

    public Tutorial GetTutorialByOrder(int Order) {
        for (int i = 0; i < Tutorials.Count; i++) {
            if (Tutorials[i].Order == Order) {
                return Tutorials[i];
            }
        }

        return null;
    }
}
