using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    public PlayerData m_player;

    [SerializeField] public string m_playerPref_Control;
    [SerializeField] public string m_playerPref_Music;

    public event Action LoadDataEvent;

    // Start is called before the first frame update

    void Start()
    {
        //Load save data
        LoadDataLocal();
        
        //Debug.Log("Load at start");

    }

    // Update is called once per frame
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Public Method

    public PlayerData GetPlayerData ()
    {
        return m_player;
        
    }

    public void LoadDataLocal()
    {

        m_player = SaveLoadManager.LoadData(SkinManager.instance.GetSkinsNumber(), AchievementManager.instance.m_achievements.Count);
        SkinManager.instance.ChangeSkin(m_player.m_usingSkin);
        Debug.Log("Load Save Data.");
        UIManager.instance.CreateAchievementMenu();
        UIManager.instance.CreateSkinMenu();
        UIManager.instance.UpdateCoinValue();
        UIManager.instance.UpdateStatPage();

    }

    public void SaveDataLocal()
    {
        if (m_player != null)
        {
            SaveLoadManager.SaveData(m_player, SkinManager.instance.GetSkinsNumber(), AchievementManager.instance.m_achievements.Count);
        }
        else
        {
            Debug.LogError("Player instance is not created");
        }

        Debug.Log("Saved Data");
    }

    public void LoadDataPlayfab ()
    {

    }

    public void SaveDataPlayfab()
    {

    }


    
    #region Setter
    public void SetUnlockSkin (int skinID)
    {
        m_player.m_unlockedSkins[skinID] = true;
    }

    public void SetAllTimeData ()
    {
        if (GameManager.instance.GetSessionDistance() > m_player.m_allTimeDist)
        {
            m_player.m_allTimeDist = (int)GameManager.instance.GetSessionDistance();
        }

        if (GameManager.instance.GetSessionCheckpoints() > m_player.m_allTimeCP)
        {
            m_player.m_allTimeCP = GameManager.instance.GetSessionCheckpoints();
        }

        AchievementManager.instance.ResetAchievementGoal();
    }

    public void SetUsingSkin(int usingSkin)
    {
        m_player.m_usingSkin = usingSkin;
    }

    public void SetUnlockAch (int achID)
    {
        m_player.m_unlockedAchievements[achID] = true;
    }

    public void ResetUnlockAch (int achID)
    {
        m_player.m_unlockedAchievements[achID] = false;
    }

    public void SetAchievementClaimed (int achID)
    {
        m_player.m_achievementClaimed[achID] = true;
    }

    public void ResetAchievementClaimed (int achID)
    {
        m_player.m_achievementClaimed[achID] = false;
    }

    public void SetTutorialPlayed ()
    {
        m_player.m_tutorialChunk = true;
    }

    #endregion

    #region Getter

    public int GetCoin ()
    {
        return m_player.m_coins;
    }

    public float GetAllTimeDist ()
    {
        return m_player.m_allTimeDist;
    }

    public int GetAllTimeCP ()
    {
        return m_player.m_allTimeCP;
    }

    public bool GetSkin (int skinID)
    {
        return m_player.m_unlockedSkins[skinID];
    }

    public int GetUsingSkin ()
    {
        return m_player.m_usingSkin;
    }

    public bool GetAchievementClaimed (int achID)
    {
        return m_player.m_achievementClaimed[achID];
    }

    public bool GetUnlockedAchievement (int achID)
    {
        //Debug.Log("Achievement ID: " + achID);
        return m_player.m_unlockedAchievements[achID];
    }

    public bool GetTutorial ()
    {
        return m_player.m_tutorialChunk;
    }

    #endregion

    #region Adjusting Data
    public void AddCoins (int amount)
    {
        m_player.m_coins += amount;
    }

    public void SubtractCoins(int amount)
    {
        m_player.m_coins -= amount;
    }
    #endregion
    

    #endregion

}
