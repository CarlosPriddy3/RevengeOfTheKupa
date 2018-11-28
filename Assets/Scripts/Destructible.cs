// --------------------------------------
// This script is totally optional. It is an example of how you can use the
// destructible versions of the objects as demonstrated in my tutorial.
// Watch the tutorial over at http://youtube.com/brackeys/.
// --------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public GameObject destroyedVersion; // Reference to the shattered version of the object
    public bool isDestroyed;

    // If the player clicks on the object
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.GetComponent<PlayerMove>().getSpinning());
        //bool isSpinning = collision.gameObject.transform.parent.GetComponent<PlayerMove>().getSpinning();
        //Debug.Log(isSpinning);
        if (collision.gameObject.tag == "Kupa" && collision.gameObject.GetComponent<PlayerMove>().getSpinning())
        {
            // Spawn a shattered object
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            // Remove the current object
            Destroy(gameObject);
            isDestroyed = true;
        }
    }
}
