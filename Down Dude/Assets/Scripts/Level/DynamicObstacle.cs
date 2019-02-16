using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DynamicObstacle : Obstacle

public class DynamicObstacle : Obstacle {

    [SerializeField] private float m_spawnDelay;          // spawn delay after trigger

    public float GetSpawnDelay()
    {
        return m_spawnDelay;
    }

}
