using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ChunkInfo
//
// - Holds chunk game object and its info when instantiated

public class ChunkInfo : MonoBehaviour {

    private int m_chunkIndex;       // index in the list of which chunk is instantiated from

    public void SetChunkIndex(int index)
    {
        m_chunkIndex = index;
    }

    // getter
    public int GetChunkIndex()
    {
        return m_chunkIndex;
    }
}
