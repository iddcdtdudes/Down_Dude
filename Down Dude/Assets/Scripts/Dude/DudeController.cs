using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DudeController : MonoBehavior
//
// manage Dude's controls
// - jetpack
// - parachute
// - idle
// - walking
// - dying

public class DudeController : MonoBehaviour {
    
    // singleton instance
    public static DudeController instance;

    public event Action reachCheckpointEvent;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // invoke checkpoint event
        if(collider.CompareTag("Checkpoint")) {
            if(reachCheckpointEvent != null) {
                reachCheckpointEvent.Invoke();
            }
        }
    }
}
