using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    [SerializeField] private int m_chunkIndex;

    [SerializeField] private float m_timeLimit;

    private float m_chunkHeight;
    [SerializeField] private GameObject m_chunkButtom;

    private void Start()
    {
        // initialize chunk height
        m_chunkHeight = Mathf.Abs(m_chunkButtom.transform.position.y);
    }

    public float GetChunkHeight()
    {
        return m_chunkHeight;
    }

    public float GetTimeLimit()
    {
        return m_timeLimit;
    }
}
