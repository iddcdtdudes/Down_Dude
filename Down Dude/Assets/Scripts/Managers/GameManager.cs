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

    [SerializeField] private Text timerText;    // timer text UI
    private float m_timer;                      // game timer

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

        // set timer to chunk's time limit
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
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
        timerText.text = (Mathf.Round(m_timer * 10) / 10).ToString();
    }

    // calls upon dude reaching checkpoint event
    private void OnDudeReachCheckpoint()
    {
        // set timer to chunk's time limit
        m_timer = ChunkManager.instance.GetNewChunkTimeLimit();
    }
}
