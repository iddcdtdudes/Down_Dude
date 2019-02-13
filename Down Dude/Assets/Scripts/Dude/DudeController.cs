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

    private DudeMode m_dudeMode;
    [SerializeField] private float m_jetpackForce;

    [SerializeField] private float m_parachuteForce;
    [SerializeField] private float m_maxParachuteForce;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }


    private void FixedUpdate()
    {

        //Debug.Log(GetComponent<Rigidbody2D>().velocity.y);

        if (Input.touchCount > 0)
        {
            //Set Character to Parachute Mode
            m_dudeMode = DudeMode.PARACHUTE;

            Touch FirstTouch = Input.GetTouch(0);

            Vector2 DudePosition = transform.position;
            Vector2 TargetPosition = CalculateTargetPosition(FirstTouch);

            Vector2 ForceDirection;
            ForceDirection.x = TargetPosition.x - DudePosition.x;
            ForceDirection.y = 0f;

            if(ForceDirection.magnitude > m_maxParachuteForce) {
                if(ForceDirection.x > 0) {
                    ForceDirection = new Vector2(m_maxParachuteForce, 0f);
                } else {
                    ForceDirection = new Vector2(-m_maxParachuteForce, 0f);
                }
            }

            GetComponent<Rigidbody2D>().AddForce(m_parachuteForce * ForceDirection, ForceMode2D.Force);
            
            
        }
        else 
        {
            //Set Character to Jetpack
            m_dudeMode = DudeMode.JETPACK;

            GetComponent<Rigidbody2D>().AddForce(Vector2.down * m_jetpackForce);
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

    public DudeMode GetDudeMode()
    {
        return m_dudeMode;
    }
}

public enum DudeMode
{ PARACHUTE, JETPACK }
