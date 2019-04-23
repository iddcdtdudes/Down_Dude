using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingCollider : MonoBehaviour {
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            DudeController.instance.SetDudeWalking(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle")) {
            DudeController.instance.SetDudeWalking(false);
        }
    }

}
