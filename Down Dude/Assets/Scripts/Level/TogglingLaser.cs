using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TogglingPart : StaticObstacle
//
// - periodically toggles activity of a referenced game object

public class TogglingLaser : StaticObstacle {

    [SerializeField] private GameObject m_laser;       // game object to toggle

    [SerializeField] private float m_upTime;                    // active time period
    [SerializeField] private float m_downTime;                  // inactive time period

    [SerializeField] private bool m_startState;

    private float m_lastToggleTime;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // initialize state
        m_laser.SetActive(m_startState);
    }

    private void Update()
    {
        // if time since last toggle exceeds up time / down time, toggle game object
        if(m_laser.activeSelf) {
            if(Time.time - m_lastToggleTime >= m_upTime) {
                Debug.Log("down");
                m_animator.SetBool("up", false);
                m_lastToggleTime = Time.time;
            }
        } else {
            if(Time.time - m_lastToggleTime >= m_downTime) {
                Debug.Log("up");
                m_animator.SetBool("up", true);
                m_lastToggleTime = Time.time;
            }
        }
    }

    public void OnLaserUp()
    {
        m_laser.SetActive(true);
    }

    public void OnLaserDown()
    {
        m_laser.SetActive(false);
    }
}
