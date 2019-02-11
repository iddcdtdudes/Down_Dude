using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Chunk : ScriptableObject
//
// used as a data container for storing a chunk and its respective data
// 
// to setup a new chunk:
// - create a Chunk scriptable object and assign info
// - add new chunk into the ChunkPrefabs[] list in ChunkManager

[CreateAssetMenu(fileName = "New Chunk", menuName = "Chunk")]
public class Chunk : ScriptableObject {

    public int chunkIndex;          // index of chunk
    public GameObject chunkPrefab;  // gameObject containing chunk

    public float chunkHeight;       // height of chunk from top to bottom
}
