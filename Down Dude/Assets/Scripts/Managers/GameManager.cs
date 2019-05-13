using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    [SerializeField] private List<GameObject> gameOverUIShow;
    [SerializeField] private List<GameObject> gameOverUIHide;

    [Header("Score and Checkpoints")]
    [SerializeField] private Text timerText;            // timer text UI
    private float m_timer;                              // game timer
    [SerializeField]private int m_timerFontSizeInit;
    [SerializeField]private int m_timerFontSizeIncre;
    [SerializeField] private Animator m_timerAnim;
    [SerializeField] private Text checkpointsText;      // checkpoints text UI
    private int m_checkpointsReached;                   // checkpoints reached
    private bool[] m_timerSoundPlayed = { false, false, false, false, false };

    [SerializeField] private Text distanceText;            // score text UI
    private float m_distance;                                // game score

    private int m_coinCollected;

    [SerializeField] private int m_scoreBaseline;                                   // base score reward for reaching a checkpoint
    [SerializeField] private int m_timeScaleMultiplier;                             // additional maximum core earned through faster play time
    [Range(0f, 0.5f)] [SerializeField] private float m_scoreMultiplierCeiling;      // time remaining percentage of maximum score multiplier

    private float m_previousDudeY;                              // dude y position on previous update
    [SerializeField] private float m_distanceScaler;      // scaler for distance

    private bool m_retrivedFirstTimeLimit = false;

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
        DudeController.instance.dudeIsKilledEvent += UIManager.instance.HideInGamePanel;
        DudeController.instance.dudeIsKilledEvent += PlayerDataManager.instance.AddDeath;
        DudeController.instance.dudeIsKilledEvent += PlayerDataManager.instance.SaveDataLocal;

        // initialize variables
        m_checkpointsReached = 0;
        m_distance = 0;
        m_coinCollected = 0;
        DudeController.instance.ResetDude();

        // initialize UI values
        timerText.text = m_timer.ToString("F1");
        checkpointsText.text = m_checkpointsReached.ToString();
        distanceText.text = m_distance.ToString();

        // initialize previous dude position
        m_previousDudeY = 0.0f;

        // play menu music
        AudioManager.instance.StopSound("Menu");
        AudioManager.instance.Play("Helicopter");
    }

    #region Update
    private void Update()
    {
        if (DudeController.instance.GetDudeState() == DudeState.NONE)
        {
            CheckTouchOnScreen();
        }

        if(!m_retrivedFirstTimeLimit) {
            m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
            m_retrivedFirstTimeLimit = true;
        }

        UpdateTimer();
        UpdateDistance();
    }
    #endregion

    #region Private
    // decrement timer
    // call once per frame
    private void UpdateTimer()
    {
        if (m_timer < 5f)
        {
            m_timerAnim.SetBool("TimerCountdown", true);
            if (m_timer < 1f)
            {
                timerText.fontSize = m_timerFontSizeInit + (10 * 9);
                if (!m_timerSoundPlayed[4]) {
                    AudioManager.instance.Play("Time Alert");
                    m_timerSoundPlayed[4] = true;
                }
            }
            else if (m_timer < 2f)
            {
                timerText.fontSize = m_timerFontSizeInit + (10 * 7);
                if (!m_timerSoundPlayed[3]) {
                    AudioManager.instance.Play("Time Alert");
                    m_timerSoundPlayed[3] = true;
                }
            }
            else if (m_timer < 3f)
            {
                timerText.fontSize = m_timerFontSizeInit + (10 * 5);
                if (!m_timerSoundPlayed[2]) {
                    AudioManager.instance.Play("Time Alert");
                    m_timerSoundPlayed[2] = true;
                }
            }
            else if (m_timer < 4f)
            {
                timerText.fontSize = m_timerFontSizeInit + (10 * 3);
                if (!m_timerSoundPlayed[1]) {
                    AudioManager.instance.Play("Time Alert");
                    m_timerSoundPlayed[1] = true;
                }
            }
            else
            {
                timerText.fontSize = m_timerFontSizeInit + (10 * 1);
                if(!m_timerSoundPlayed[0]) {
                    AudioManager.instance.Play("Time Alert");
                    m_timerSoundPlayed[0] = true;
                }
            }
        }
        else
        {
            m_timerAnim.SetBool("TimerCountdown", false);
            timerText.fontSize = m_timerFontSizeInit;
            for (int i = 0; i < m_timerSoundPlayed.Length; i++)
            {
                m_timerSoundPlayed[i] = false;
            }
        }
        // decrement timer
        if (m_timer > 0f) {
            m_timer -= Time.deltaTime;
        } else {
            DudeController.instance.KillDude();
        }

        // update timer text UI
        timerText.text = m_timer.ToString("F1");
    }

    private void CheckTouchOnScreen ()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Debug.Log("Found Touch");
            //Make sure finger is NOT over a UI element
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //Debug.Log("Touch on Screen");
                StartGame();
            }
            //else
            //{
            //    Debug.Log("Touch on UI");
            //}
        }
    }

    // add amount to score
    private void UpdateDistance()
    {
        // update delta y
        float deltaY = m_previousDudeY - DudeController.instance.transform.position.y;
        m_previousDudeY = DudeController.instance.transform.position.y;

        // increment distance
        m_distance += deltaY * m_distanceScaler;

        // update score text UI
        distanceText.text = m_distance.ToString("F0") + 'm';
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
        AudioManager.instance.Play("Checkpoint");

        // calculate and add score
        float chunkTimeLimit = ChunkManager.instance.GetChunkList()[ChunkManager.instance.GetChunkIndex(1)].GetTimeLimit();
        float timeScale = Mathf.Clamp01((m_timer / chunkTimeLimit) / m_scoreMultiplierCeiling);

        // set timer to chunk's time limit
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
    }
    #endregion

    #region Public

    public float GetSessionDistance ()
    {
        return m_distance;
    }

    public int GetSessionCheckpoints ()
    {
        return m_checkpointsReached;
    }

    public int GetSessionCoins ()
    {
        return m_coinCollected;
    }

    public void AddSessionCoin ()
    {
        m_coinCollected += 1;
        //Debug.Log("Session Coin: " + m_coinCollected);
    }

    public void ResumeGame ()
    {
        AudioManager.instance.Play("BGM");
        AudioManager.instance.StopSound("Menu");
        Time.timeScale = 1f;
        DudeController.instance.SetDudeState(DudeState.ALIVE);
    }

    public void StartGame ()
    {
        UIManager.instance.HideMenu();
        StartSequence.instance.GameStart();
        AudioManager.instance.Play("Door");
    }

    public void OnDudeJump()
    {
        AudioManager.instance.StopSound("Helicopter");
        //AudioManager.instance.Play("BGM");
        if (PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Music))
        {
            //Play("Theme");
            if (PlayerPrefs.GetInt(PlayerDataManager.instance.m_playerPref_Music) == 1)
            {
                AudioManager.instance.Play("BGM");
                AudioManager.instance.Music(true);
            }
            else
            {

                AudioManager.instance.Music(false);
            }
        }
        else
        {
            //Play("Theme");
            AudioManager.instance.Music(true);
            AudioManager.instance.Play("BGM");
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Music, 1);
            PlayerPrefs.Save();
            Debug.Log("No music key");
        }

        ChunkManager.instance.GameStart();
        DudeController.instance.SetDudeState(DudeState.ALIVE);
        DudeController.instance.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;

        BackgroundManager.instance.GameStart();

        Time.timeScale = 1f;
    }

    public void PauseGame ()
    {
        //synchronize music
        AudioManager.instance.Play("Menu");
        AudioManager.instance.SynchronizeAudio("BGM", "Menu");
        AudioManager.instance.StopSound("BGM");
        Time.timeScale = 0.0f;
        DudeController.instance.SetDudeState(DudeState.NONE);
    }

    public void GameOverUI ()
    {
        //StartCoroutine("GameOverScreen");
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

    #endregion

}
