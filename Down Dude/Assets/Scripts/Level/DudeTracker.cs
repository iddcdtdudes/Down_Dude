using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DudeTracker : DynamicObstacle
//
// - tracks dude for x seconds

public class DudeTracker : DynamicObstacle
{
    [SerializeField] private float m_moveSpeed;         // moving speed
    [SerializeField] private float m_steerSpeed;        // steering speed
    [SerializeField] private float m_trackingTime;      // time limit for tracking since missile becomes active
    private float m_deactivateTime;                     // time the missile deactivates

    // references
    private Transform m_dude;
    private Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_dude = DudeController.instance.transform;
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_deactivateTime = Time.time + m_trackingTime;
    }

    private void FixedUpdate()
    {
        // if missile is still tracking
        if(Time.time < m_deactivateTime) {

            // rotate toward dude
            float steer = Vector3.Cross(transform.up, m_dude.transform.position - transform.position).z;
            transform.Rotate(0f, 0f, steer * m_steerSpeed);
        }

        // move forward
        m_rigidbody.velocity = transform.up * m_moveSpeed;
    }
}
