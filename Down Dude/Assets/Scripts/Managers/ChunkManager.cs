using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ChunkManager : MonoBehaviour
//
// provides chunk spawning / despawning interface

public class ChunkManager : MonoBehaviour {

    // singleton instance
    public static ChunkManager instance;

    [SerializeField] private int m_chunkListCount;      // coun of chunks in chunkList
    [SerializeField] private Chunk[] m_chunkList;       // list of all chunk scriptable objects available

    private int m_firstChunkIndex;                      // index of first chunk
    [SerializeField] private int m_loadedChunksLimit;
    private List<GameObject> m_loadedChunks;            // chunks loaded

    private float m_newChunkTimeLimit;                  // time limit of newly spawned chunk

    private float m_previousChunkHeight;                // height of the latest loaded chunk
    [SerializeField] private float spawnOffset;         // distance of Dude from the current chunk's checkpoint to spawn the next chunk

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // randomize index of first chunk
        m_firstChunkIndex = Random.Range(0, m_chunkListCount);
        m_newChunkTimeLimit = m_chunkList[m_firstChunkIndex].timeLimit;
    }

    private void Start()
    {
        // initialize loaded chunk list
        m_loadedChunks = new List<GameObject>();

        // add first chunk
        PushChunk(m_firstChunkIndex);
    }

    private void Update()
    {
        // spawn chunk if ready
        if(ReadyToSpawnChunk()) {
            PushChunk(Random.Range(0, m_chunkListCount));

            // if chunk count exceeds limit, pop a chunk
            if(m_loadedChunks.Count > m_loadedChunksLimit) {
                PopChunk();
            }
        }
    }

    // returns true if next chunk must be spawned
    private bool ReadyToSpawnChunk()
    {
        // y position of Dude
        float dudeY = DudeController.instance.transform.position.y;

        // y position of Dude that should spawn a chunk
        // equals to the the next checkpoint - spawnOffset
        float spawningY = m_loadedChunks[m_loadedChunks.Count - 1].transform.position.y - m_previousChunkHeight + spawnOffset;

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
        m_loadedChunks.Add(Instantiate(m_chunkList[index].chunkPrefab));
        m_loadedChunks[m_loadedChunks.Count - 1].transform.parent = this.transform;

        // position chunk
        if(m_loadedChunks.Count >= 2) {
            float previousChunkY = m_loadedChunks[m_loadedChunks.Count - 2].transform.position.y;
            m_loadedChunks[m_loadedChunks.Count - 1].transform.position = new Vector3(0f, previousChunkY - m_previousChunkHeight, 0f);
        } else {
            m_loadedChunks[m_loadedChunks.Count - 1].transform.position = Vector3.zero;
        }

        // update class states
        m_previousChunkHeight = m_chunkList[index].chunkHeight;
        m_newChunkTimeLimit = m_chunkList[index].timeLimit;
    }

    // despawn the top most chunk
    private void PopChunk()
    {
        // destroy chunk and remove from list
        Destroy(m_loadedChunks[0]);
        m_loadedChunks.RemoveAt(0);
    }

    public float GetNewChunkTimeLimit()
    {
        return m_newChunkTimeLimit;
    }
}
