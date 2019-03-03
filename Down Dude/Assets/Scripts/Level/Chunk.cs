using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    [SerializeField] private int m_chunkIndex;

    [SerializeField] private float m_timeLimit;

    private float m_chunkHeight;
    [SerializeField] private GameObject m_chunkTop;
    [SerializeField] private GameObject m_chunkButtom;

    private void Start()
    {
        // initialize chunk height
        m_chunkHeight = Mathf.Abs(m_chunkTop.transform.position.y - m_chunkButtom.transform.position.y);
    }

    public int GetChunkIndex()
    {
        return m_chunkIndex;
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
