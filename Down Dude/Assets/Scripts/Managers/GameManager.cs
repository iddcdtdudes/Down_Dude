using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GameManager : MonoBehabiour
//
// keeps track and control states of the game
// - timer
// - scoring
// - checkpoints
// - chunk manager's activities

public class GameManager : MonoBehaviour {

    // singleton instance
    public static GameManager instance;

    [SerializeField] private Text timerText;            // timer text UI
    private float m_timer;                              // game timer

    [SerializeField] private Text checkpointsText;      // checkpoints text UI
    private int m_checkpointsReached;                   // checkpoints reached

    [SerializeField] private Text scoreText;            // score text UI
    private int m_score;                                // game score

    [SerializeField] private int m_scoreBaseline;                                   // base score reward for reaching a checkpoint
    [SerializeField] private int m_timeScaleMultiplier;                             // additional maximum core earned through faster play time
    [Range(0f, 0.5f)] [SerializeField] private float m_scoreMultiplierCeiling;      // time remaining percentage of maximum score multiplier

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // event subscription
        DudeController.instance.reachCheckpointEvent += OnDudeReachCheckpoint;

        // initialize variables
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
        m_checkpointsReached = 0;
        m_score = 0;

        // initialize UI values
        timerText.text = m_timer.ToString("F1");
        checkpointsText.text = m_checkpointsReached.ToString();
        scoreText.text = m_score.ToString();
    }

    private void Update()
    {
        UpdateTimer();
    }

    // decrement timer
    // call once per frame
    private void UpdateTimer()
    {
        // decrement timer
        if(m_timer > 0f) {
            m_timer -= Time.deltaTime;
        } else {
            // TODO: trigger game over
        }

        // update timer text UI
        timerText.text = m_timer.ToString("F1");
    }

    // add amount to score
    private void AddScore(int amount)
    {
        // add to score
        m_score += amount;

        // update score text UI
        scoreText.text = m_score.ToString();
    }

    // add 1 to checkpoints reached
    private void incrementCheckpoints()
    {
        // increment checkpoints reached
        m_checkpointsReached++;

        // update checkpoints text UI
        checkpointsText.text = m_checkpointsReached.ToString();
    }

    // calls upon dude reaching checkpoint event
    private void OnDudeReachCheckpoint()
    {
        // increment Checkpoint
        incrementCheckpoints();

        // calculate and add score
        float chunkTimeLimit = ChunkManager.instance.GetChunkList()[ChunkManager.instance.GetChunkIndex(1)].timeLimit;
        float timeScale = Mathf.Clamp01((m_timer / chunkTimeLimit) / m_scoreMultiplierCeiling);
        AddScore(m_scoreBaseline + (int)(timeScale * m_timeScaleMultiplier));
        
        // set timer to chunk's time limit
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();

    }
}
