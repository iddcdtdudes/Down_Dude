using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MovingObstacle : DynamicObstacle
//
// - translate obstacle by velocity


public class MovingObstacle : DynamicObstacle {

    [SerializeField] private Vector2 m_velocity;    // moving velocity of obstacle

    protected bool m_isStopped = false;               // obstacle is stopped by another obstacle

    private void Update()
    {
        // translate by velocity
        if(!m_isStopped) {
            transform.Translate(m_velocity * Time.deltaTime);
        }
    }

    public bool GetIsStopped()
    {
        return m_isStopped;
    }
}
