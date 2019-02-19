using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PinchingObstacle : MovingObstacle
//
//

public class PinchingObstacle : MovingObstacle {

    public event Action DudeCollisionEnterEvent;
    public event Action DudeCollisionExitEvent;

    private bool isCollidingDude = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // invoke on collision enter Dude
        if(collision.gameObject.CompareTag("Dude")) {
            if(DudeCollisionEnterEvent != null) {
                DudeCollisionEnterEvent.Invoke();
            }
        }

        // stop on colliding with another pincher
        PinchingObstacle other = collision.gameObject.GetComponent<PinchingObstacle>();
        if(other != null) {
            m_isStopped = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // invoke on collision exit Dude
        if(collision.gameObject.CompareTag("Dude")) {
            if(DudeCollisionExitEvent != null) {
                DudeCollisionExitEvent.Invoke();
            }
        }
    }
}
