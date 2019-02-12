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
    [SerializeField] private int m_loadedChunksLimit;   // limit of chunks loaded at once
    private List<GameObject> m_loadedChunks;            // chunks loaded

    [SerializeField] private float spawnOffset;         // distance of Dude from the current chunk's checkpoint to spawn the next chunk

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // initialize loaded chunk list
        m_loadedChunks = new List<GameObject>();

        // randomize index of first chunk
        m_firstChunkIndex = Random.Range(0, m_chunkListCount);
    }

    private void Start()
    {
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
        float spawningY = m_loadedChunks[m_loadedChunks.Count - 1].transform.position.y - m_chunkList[GetChunkIndex(0)].chunkHeight + spawnOffset;

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

        // add chunk info component and set index
        m_loadedChunks[m_loadedChunks.Count - 1].AddComponent<ChunkInfo>();
        m_loadedChunks[m_loadedChunks.Count - 1].GetComponent<ChunkInfo>().SetChunkIndex(index);

        // set parent
        m_loadedChunks[m_loadedChunks.Count - 1].transform.parent = this.transform;

        // position chunk
        if(m_loadedChunks.Count >= 2) {
            float previousChunkY = m_loadedChunks[m_loadedChunks.Count - 2].transform.position.y;
            float previousChunkHeight = m_chunkList[GetChunkIndex(1)].chunkHeight;

            m_loadedChunks[m_loadedChunks.Count - 1].transform.position = new Vector3(0f, previousChunkY - previousChunkHeight, 0f);
        } else {
            m_loadedChunks[m_loadedChunks.Count - 1].transform.position = Vector3.zero;
        }
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
            return m_chunkList[GetChunkIndex(0)].timeLimit;
        } else {
            return m_chunkList[m_firstChunkIndex].timeLimit;
        }
    }

    // returns the chunk index of the loaded chunk
    // chunk = 0 : latest chunk; +1 every older chunk
    public int GetChunkIndex(int chunk)
    {
        if(m_loadedChunks.Count - chunk - 1 >= 0) {
            return m_loadedChunks[m_loadedChunks.Count - chunk - 1].GetComponent<ChunkInfo>().GetChunkIndex();
        } else {
            return -1;
        }
    }

    public Chunk[] GetChunkList()
    {
        return m_chunkList;
    }
}
