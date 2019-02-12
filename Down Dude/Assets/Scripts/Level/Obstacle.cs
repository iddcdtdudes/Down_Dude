using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Obstacle : MonoBehaviour
//
// holds obstacle information

public class Obstacle : MonoBehaviour {

    [SerializeField] private float m_spawnDelay;          // spawn delay after trigger

    public float GetSpawnDelay()
    {
        return m_spawnDelay;
    }
}
