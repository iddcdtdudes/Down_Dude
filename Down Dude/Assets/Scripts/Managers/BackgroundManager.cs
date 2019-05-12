using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BackgroundManager : MonoBehavior
//
// - controls the background states

public class BackgroundManager : MonoBehaviour {

    public static BackgroundManager instance;   // singleton instance

    [SerializeField] private List<BackgroundThemeHolder> m_backgrounds;     // list of background themes
    [SerializeField] private BackgroundLooper m_transition;                 // transition background

    private BackgroundTheme m_currentTheme;     // current background theme

    [SerializeField] private int m_checkpointsUntilChange;      // checkpoints count until theme change
    private int m_checkpointsCount;                             // checkpoints count since last background change

    private bool m_backgroundTransitioning = false;             // transitioning

    [SerializeField] private int m_startingBackground = -1;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start()
    {
        DudeController.instance.reachCheckpointEvent += OnDudeReachCheckpoint;

        BackgroundTheme newTheme;
        if (m_startingBackground < 0 || m_startingBackground >= m_backgrounds.Count) {
            newTheme = (BackgroundTheme)Random.Range(0, m_backgrounds.Count);
        } else {
            newTheme = (BackgroundTheme)m_startingBackground;
        }

        SetBackgroundTheme(newTheme);
        m_backgrounds[(int)m_currentTheme].InitializePositions();

        m_transition.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) {
            RandomizeBackgroundTheme();
        }
    }

    // randomize background theme
    // - no repeating themes
    public void RandomizeBackgroundTheme()
    {
        // stop if already transitioning
        if(m_backgroundTransitioning) {
            return;
        }

        // loop until backgound does not repeat
        bool changed = false;
        while(!changed) {
            // randomize a theme index
            BackgroundTheme newTheme = (BackgroundTheme)Random.Range(0, m_backgrounds.Count);

            // if not current theme, change theme
            if(m_currentTheme != newTheme) {
                StartCoroutine(SetBackgroundThemeWithTransition(newTheme));
                m_currentTheme = newTheme;
                changed = true;
            }
        }
    }

    private IEnumerator SetBackgroundThemeWithTransition(BackgroundTheme theme)
    {
        m_backgroundTransitioning = true;

        // initialize transition
        InitializeTransition();
        m_transition.gameObject.SetActive(true);

        bool changed = false;
        bool transitioning = true;
        while(transitioning) {

            // if dude move pass a threshold, change background
            if (!changed) {
                if (DudeController.instance.transform.position.y < m_transition.GetBackgroundY() - 5.0f) {
                    SetBackgroundTheme(theme);
                    changed = true;
                }
            }
            
            // if dude move pass a threashold, break from loop
            else {
                if (DudeController.instance.transform.position.y < m_transition.GetBackgroundY() - 12.0f) {
                    transitioning = false;
                }
            }

            yield return null;
        }

        // deactivate transition
        m_transition.gameObject.SetActive(false);

        m_backgroundTransitioning = false;
    }

    // set background theme
    private void SetBackgroundTheme(BackgroundTheme theme)
    {
        foreach(BackgroundThemeHolder background in m_backgrounds) {
            background.SetStatus(false);
        }
        m_currentTheme = theme;
        m_backgrounds[(int)m_currentTheme].SetStatus(true);
        m_backgrounds[(int)m_currentTheme].InitializePositions();
    }

    private void OnDudeReachCheckpoint()
    {
        m_checkpointsCount++;
        if(m_checkpointsCount >= m_checkpointsUntilChange) {
            RandomizeBackgroundTheme();
            m_checkpointsCount = 0;
        }
    }

    private void InitializeTransition()
    {
        m_transition.InitializePosition(true);
    }

    public void GameStart()
    {
        foreach(BackgroundThemeHolder theme in m_backgrounds) {
            theme.SetRising(false);
        }
        m_transition.SetRising(false);
    }
}

public enum BackgroundTheme { CAVE = 0, FOREST, CITY, VALLEY };

// Background Theme
//
// - holds 3 background layers

[System.Serializable]
class BackgroundThemeHolder
{
    public BackgroundLooper m_bgLayer0;   // background layer 0
    public BackgroundLooper m_bgLayer1;   // background layer 1
    public BackgroundLooper m_bgLayer2;   // background layer 2

    // set active for all background layers
    public void SetStatus(bool active)
    {
        m_bgLayer0.gameObject.SetActive(active);
        m_bgLayer1.gameObject.SetActive(active);
        m_bgLayer2.gameObject.SetActive(active);

        if(active) {
            m_bgLayer0.SetFirstActivated(true);
            m_bgLayer1.SetFirstActivated(true);
            m_bgLayer2.SetFirstActivated(true);
        }
    }

    // initialize positions for all background layers
    public void InitializePositions()
    {
        m_bgLayer0.InitializePosition(false);
        m_bgLayer1.InitializePosition(false);
        m_bgLayer2.InitializePosition(false);
    }

    // rising
    public void SetRising(bool value)
    {
        m_bgLayer0.SetRising(value);
        m_bgLayer1.SetRising(value);
        m_bgLayer2.SetRising(value);
    }
}
