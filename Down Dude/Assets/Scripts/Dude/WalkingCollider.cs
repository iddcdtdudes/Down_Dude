using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingCollider : MonoBehaviour {
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Reached Obstacle");
            DudeController.instance.SetDudeWalking(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        DudeController.instance.SetDudeWalking(false);
    }

}
