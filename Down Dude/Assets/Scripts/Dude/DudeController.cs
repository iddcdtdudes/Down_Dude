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

    public bool m_dudeJetpackMode;
    public bool m_dudeParachuteMode;

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            //Set Character to Parachute Mode
            m_dudeJetpackMode = false;
            m_dudeParachuteMode = true;

            Touch FirstTouch = Input.GetTouch(0);
            while (FirstTouch.phase != TouchPhase.Ended)
            {
                Vector2 TargetPosition = CalculateTargetPosition(FirstTouch);
                transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Time.deltaTime);
            }

            //Set Character Back to Jetpack Mode
            if (FirstTouch.phase == TouchPhase.Ended)
            {
                m_dudeJetpackMode = true;
                m_dudeParachuteMode = false;
            }
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

    private Vector2 CalculateTargetPosition(Touch FirstTouch)
    {
        Vector2 TargetPosition;
        Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position);

        TargetPosition.x = TouchPosition.x;
        TargetPosition.y = transform.position.y;

        return TargetPosition;
    }

}
