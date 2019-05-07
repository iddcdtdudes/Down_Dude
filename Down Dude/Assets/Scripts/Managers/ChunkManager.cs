﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ChunkManager : MonoBehaviour
//
// provides chunk spawning / despawning interface

public class ChunkManager : MonoBehaviour {

    // singleton instance
    public static ChunkManager instance;
    
    [SerializeField] private Chunk[] m_chunkList;       // list of all chunk scriptable objects available

    private int m_firstChunkIndex;                      // index of first chunk
    [SerializeField] private int m_loadedChunksLimit;   // limit of chunks loaded at once
    private List<GameObject> m_loadedChunks;            // chunks loaded

    [SerializeField] private float spawnOffset;         // distance of Dude from the current chunk's checkpoint to spawn the next chunk

    private float m_lastSpawnTime;

    // DEBUG
    [Header("Chunk Debugging")]
    [SerializeField] private bool m_chunkDebugger;      // spawn specific chunk for debugging
    [SerializeField] private int m_chunkToDebug;        // chunk to spawn when debugging

    private bool m_spawning = false;             // spawning chunk

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // clamp chunkToDebug
        if(m_chunkToDebug < 0) {
            m_chunkToDebug = 0;
        } else if(m_chunkToDebug >= m_chunkList.Length) {
            m_chunkToDebug = m_chunkList.Length - 1;
        }

        // initialize loaded chunk list
        m_loadedChunks = new List<GameObject>();

        // randomize index of first chunk
        if(!m_chunkDebugger) {
            m_firstChunkIndex = Random.Range(1, m_chunkList.Length);
        } else {
            m_firstChunkIndex = m_chunkToDebug;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        // spawn chunk if ready
        if(m_spawning && ReadyToSpawnChunk()) {
            if(!m_chunkDebugger) {
                PushChunk(Random.Range(1, m_chunkList.Length));

            } else {
                PushChunk(m_chunkToDebug);
            }

            // if chunk count exceeds limit, pop a chunk
            if(m_loadedChunks.Count > m_loadedChunksLimit) {
                PopChunk();
            }
        }
    }

    public void GameStart()
    {
        m_spawning = true;

        // add first chunk
        PushChunk(m_firstChunkIndex);
    }

    // returns true if next chunk must be spawned
    private bool ReadyToSpawnChunk()
    {
        // if not passed delay time, return false
        if(Time.time < m_lastSpawnTime + 1f) {
            return false;
        }

        // y position of Dude
        float dudeY = DudeController.instance.transform.position.y;

        // y position of Dude that should spawn a chunk
        // equals to the the next checkpoint - spawnOffset
        float spawningY = GetLoadedChunk(0).transform.position.y - GetLoadedChunk(0).GetChunkHeight() + spawnOffset;

        // if dude has surpassed the point, chunk is ready to spawn
        if(dudeY <= spawningY) {
            return true;
        } else {
            return false;
        }
    }

    // spawn a chunk
    private void PushChunk(int index)
    {
        // instantiate chunk
        m_loadedChunks.Add(Instantiate(m_chunkList[index].gameObject));

        // set parent
        m_loadedChunks[m_loadedChunks.Count - 1].transform.parent = this.transform;

        // position chunk
        if(m_loadedChunks.Count >= 2) {
            float previousChunkY = GetLoadedChunk(1).transform.position.y;
            float previousChunkHeight = GetLoadedChunk(1).GetChunkHeight();

            GetLoadedChunk(0).transform.position = new Vector3(0f, previousChunkY - previousChunkHeight, 0f);
        } else {
            GetLoadedChunk(0).transform.position = Vector3.zero;
        }

        // update last spawn time
        m_lastSpawnTime = Time.time;
    }

    // despawn the top most chunk
    private void PopChunk()
    {
        // destroy chunk and remove from list
        Destroy(m_loadedChunks[0]);
        m_loadedChunks.RemoveAt(0);
    }

    // returns the latest spawned chunk's time limit
    public float GetNewChunkTimeLimit()
    {
        if(m_loadedChunks.Count > 0) {
            Debug.Log(GetChunkIndex(0));
            return m_chunkList[GetChunkIndex(0)].GetTimeLimit();
        } else {
            return m_chunkList[m_firstChunkIndex].GetTimeLimit();
        }
    }

    // returns the chunk index of the loaded chunk
    // chunk = 0 : latest chunk; +1 every older chunk
    public int GetChunkIndex(int chunk)
    {
        if(m_loadedChunks.Count - chunk - 1 >= 0) {
            return m_loadedChunks[m_loadedChunks.Count - chunk - 1].GetComponent<Chunk>().GetChunkIndex();
        } else {
            return -1;
        }
    }

    public Chunk GetLoadedChunk(int chunk)
    {
        return m_loadedChunks[m_loadedChunks.Count - chunk - 1].GetComponent<Chunk>();
    }

    public Chunk[] GetChunkList()
    {
        return m_chunkList;
    }
}
