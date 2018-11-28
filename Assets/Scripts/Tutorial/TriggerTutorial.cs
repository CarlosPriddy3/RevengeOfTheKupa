using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorial {

    private bool isCurrentTutorial = false;
    private int count = 0;

    public Transform HitTransform;

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;
    }

    public void OnTriggerEnter(Collider c)
    {
        count++;
        if (count == 1)
        {
            TutorialManager.Instance.CompletedTutorial();
            isCurrentTutorial = false;
        }
    }
}
