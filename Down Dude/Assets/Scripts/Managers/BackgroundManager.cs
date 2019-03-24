using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BackgroundManager : MonoBehavior
//
// - controls the background states

public class BackgroundManager : MonoBehaviour {

    public static BackgroundManager instance;   // singleton instance

    [SerializeField] private List<BackgroundThemeHolder> m_backgrounds;  // list of background themes

    private BackgroundTheme m_currentTheme;     // current background theme

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
        RandomizeBackgroundTheme();
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
        // loop until backgound does not repeat
        bool changed = false;
        while(!changed) {
            // randomize a theme index
            BackgroundTheme newTheme = (BackgroundTheme)Random.Range(0, m_backgrounds.Count);

            // if not current theme, change theme
            if(m_currentTheme != newTheme) {
                SetBackgroundTheme(newTheme);
                m_currentTheme = newTheme;
                changed = true;
            }
        }
    }

    // set background theme
    private void SetBackgroundTheme(BackgroundTheme theme)
    {
        foreach(BackgroundThemeHolder background in m_backgrounds) {
            background.SetStatus(false);
            Debug.Log("x");
        }
        m_currentTheme = theme;
        m_backgrounds[(int)m_currentTheme].SetStatus(true);
        m_backgrounds[(int)m_currentTheme].InitializePositions();
    }
}

public enum BackgroundTheme { BLUE = 0, RED = 1 };

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
    }

    // initialize positions for all background layers
    public void InitializePositions()
    {
        m_bgLayer0.InitializePosition();
        m_bgLayer1.InitializePosition();
        m_bgLayer2.InitializePosition();
    }
}
