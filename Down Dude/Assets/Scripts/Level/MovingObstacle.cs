using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MovingObstacle : DynamicObstacle
//
// - translate obstacle by velocity


public class MovingObstacle : DynamicObstacle {

    [SerializeField] private Vector2 m_velocity;      // moving velocity of obstacle

    private void Update()
    {
        // translate by velocity
        transform.Translate(m_velocity * Time.deltaTime);
    }
}
