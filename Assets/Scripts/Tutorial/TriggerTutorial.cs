using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorial {

    private bool isCurrentTutorial = false;

    public Transform HitTransform;

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.GetComponent<Destructible>().isDestroyed);
        if (collision.gameObject.tag == "Crate" && collision.gameObject.GetComponent<Destructible>().isDestroyed)
        {
            TutorialManager.Instance.CompletedTutorial();
            isCurrentTutorial = false;
        }
    }
}
