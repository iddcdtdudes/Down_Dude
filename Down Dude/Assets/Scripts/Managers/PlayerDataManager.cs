using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    public PlayerData m_player;

    // Start is called before the first frame update
    void Start()
    {
        //Load save data
        //if (m_player == null)
        //{
            LoadDataLocal();
            Debug.Log("Load at start");
        //}

        //Event Subscription
        DudeController.instance.dudeIsKilledEvent += SetAllTimeData;
        DudeController.instance.dudeIsKilledEvent += SaveDataLocal;

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

        m_player = SaveLoadManager.LoadData(SkinManager.instance.GetSkinsNumber());
        Debug.Log("Load Save Data.");

    }

    public void SaveDataLocal()
    {
        if (m_player != null)
        {
            SaveLoadManager.SaveData(m_player, SkinManager.instance.GetSkinsNumber());
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
        if (GameManager.instance.GetSessionScores() > m_player.m_allTimeHS)
        {
            m_player.m_allTimeHS = GameManager.instance.GetSessionScores();
        }

        if (GameManager.instance.GetSessionCheckpoints() > m_player.m_allTimeCP)
        {
            m_player.m_allTimeCP = GameManager.instance.GetSessionCheckpoints();
        }
    }

    #endregion

    #region Getter
    public int GetCoin ()
    {
        return m_player.m_coins;
    }

    public int GetAllTimeHS ()
    {
        return m_player.m_allTimeHS;
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
    #endregion

    #region Setter

    public void SetUsingSkin (int usingSkin)
    {
        m_player.m_usingSkin = usingSkin;
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
