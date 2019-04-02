using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField]private bool GameStart;

    [SerializeField] private Text timerText;            // timer text UI
    private float m_timer;                              // game timer

    [SerializeField] private Text checkpointsText;      // checkpoints text UI
    private int m_checkpointsReached;                   // checkpoints reached

    [SerializeField] private Text scoreText;            // score text UI
    private int m_score;                                // game score

    [SerializeField] private List<GameObject> gameOverUIShow;

    [SerializeField] private List<GameObject> gameOverUIHide;

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
        if (GameStart == false)
        {
            Time.timeScale = 0f;
        }

        // set target framerate
        Application.targetFrameRate = 60;

        // set device orientation
        Screen.orientation = ScreenOrientation.Portrait;

        // event subscription
        DudeController.instance.reachCheckpointEvent += OnDudeReachCheckpoint;
        DudeController.instance.reachCheckpointEvent += AchievementManager.instance.UpdateAchProgress;
        DudeController.instance.dudeIsKilledEvent += PlayerDataManager.instance.SetAllTimeData;
        DudeController.instance.dudeIsKilledEvent += AchievementManager.instance.ResetAchProgress;
        DudeController.instance.dudeIsKilledEvent += UIManager.instance.UpdateGameOverUI;
        DudeController.instance.dudeIsKilledEvent += GameOverUI;
        DudeController.instance.dudeIsKilledEvent += PlayerDataManager.instance.SaveDataLocal;

        // initialize variables
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
        m_checkpointsReached = 0;
        m_score = 0;
        DudeController.instance.ResetDude();

        // initialize UI values
        timerText.text = m_timer.ToString("F1");
        checkpointsText.text = m_checkpointsReached.ToString();
        scoreText.text = m_score.ToString();
    }

    #region Update
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
            DudeController.instance.KillDude();
        }

        // update timer text UI
        timerText.text = m_timer.ToString("F1");
    }
    #endregion

    #region Private
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
        PlayerDataManager.instance.AddCoins(1);
        AudioManager.instance.Play("Checkpoint");

        // calculate and add score
        float chunkTimeLimit = ChunkManager.instance.GetChunkList()[ChunkManager.instance.GetChunkIndex(1)].GetTimeLimit();
        float timeScale = Mathf.Clamp01((m_timer / chunkTimeLimit) / m_scoreMultiplierCeiling);
        AddScore(m_scoreBaseline + (int)(timeScale * m_timeScaleMultiplier));

        // set timer to chunk's time limit
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
    }
    #endregion

    public int GetSessionScores ()
    {
        return m_score;
    }

    public int GetSessionCheckpoints ()
    {
        return m_checkpointsReached;
    }

    public void StartGame ()
    {
        Time.timeScale = 1f;
    }

    public void GameOverUI ()
    {
        foreach (GameObject i in gameOverUIShow)
        {
            i.SetActive(true);
        }

        foreach (GameObject i in gameOverUIHide)
        {
            i.SetActive(false);
        }
    }

    public void RestartGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
