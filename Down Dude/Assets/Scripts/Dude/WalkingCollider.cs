using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingCollider : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            DudeController.instance.SetDudeWalking(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            DudeController.instance.SetDudeWalking(false);
        }
    }
}
