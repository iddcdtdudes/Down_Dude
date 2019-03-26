using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TogglingPart : StaticObstacle
//
// - periodically toggles activity of a referenced child game object

public class TogglingPart : StaticObstacle {

    [SerializeField] private GameObject m_togglingObject;       // game object to toggle

    [SerializeField] private float m_upTime;                    // active time period
    [SerializeField] private float m_downTime;                  // inactive time period

    [SerializeField] private bool m_startState;

    private float m_lastToggleTime;

    private void Start()
    {
        // initialize state
        m_togglingObject.SetActive(m_startState);
    }

    private void Update()
    {
        // if time since last toggle exceeds up time / down time, toggle game object
        if(m_togglingObject.activeSelf) {
            if(Time.time - m_lastToggleTime >= m_upTime) {
                m_togglingObject.SetActive(false);
                m_lastToggleTime = Time.time;
            }
        } else {
            if(Time.time - m_lastToggleTime >= m_downTime) {
                m_togglingObject.SetActive(true);
                m_lastToggleTime = Time.time;
            }
        }

    }


}
