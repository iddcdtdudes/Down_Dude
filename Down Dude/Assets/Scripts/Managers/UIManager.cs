using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [Header("Game Over UI")]
    public Text m_sessionHS;
    public Text m_allTimeHS;
    public Text m_sessionCP;
    public Text m_allTimeCP;
    //public Text m_achievementCompleted;
    public GameObject m_achGameOverPanel;
    public GameObject m_achGameOverPrefab;

    [Header("Achievement UI")]
    public GameObject m_achievementPanel;
    public GameObject m_achievementPrefab;

    [Header("Menu")]
    public Text m_coins;
    

    // Start is called before the first frame update
    void Awake()
    {
        // initialize singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Set achievement according to playerdata
        //AchievementManager.instance.LoadFromPlayerdata();

        //Update Coins in menu
        UpdateCoinValue();

        //Create each achievement list in UI
        
    }

    #region Main Menu

    public void UpdateCoinValue ()
    {
        //Update Coins in menu
        m_coins.text = PlayerDataManager.instance.GetCoin().ToString();
    }

    #endregion

    #region Gameover UI

    public void UpdateGameOverUI()
    {
        m_sessionHS.text = GameManager.instance.GetSessionScores().ToString();
        m_sessionCP.text = GameManager.instance.GetSessionCheckpoints().ToString();
        m_allTimeHS.text = PlayerDataManager.instance.GetAllTimeHS().ToString();
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
    }

    public void CreateAchInGameOverUI (string description)
    {
        //Create new gameobject from the prefab
        GameObject achGameOverPrefab = Instantiate(m_achGameOverPrefab);

        //Set Data to Prefab
        SetAchGameOverInfo(m_achGameOverPanel, achGameOverPrefab, description);
    }

    private void SetAchGameOverInfo (GameObject achParent, GameObject achPrefab, string description)
    {
        achPrefab.transform.SetParent(achParent.transform);
        achPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Text achDescription = achPrefab.GetComponentInChildren<Text>();
        achDescription.text = description;
    }

    #endregion

    #region Achievement Panel

    public void CreateAchievementMenu ()
    {
        for (int i = 0; i < AchievementManager.instance.m_achievements.Count; i++)
        {
            CreateAchievement(m_achievementPanel, AchievementManager.instance.m_achievements[i]);
        }
    }

    private void CreateAchievement (GameObject achParent, AchievementObject achData)
    {
        GameObject achPrefab = Instantiate(m_achievementPrefab);

        SetAchievementInfo(achParent, achPrefab , achData);

        Debug.Log("Create achievement in menu");
    }

    private void SetAchievementInfo (GameObject achParent, GameObject achPrefab, AchievementObject achData)
    {
        achPrefab.transform.SetParent(achParent.transform);
        achPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        AchievementUI achievementPrefab = achPrefab.GetComponent<AchievementUI>();
        achievementPrefab.SetTitle(achData.ach_Title);
        achievementPrefab.SetDescription(achData.ach_Description);
        achievementPrefab.SetReward(achData.ach_Reward);

        if (PlayerDataManager.instance.GetUnlockedAchievement(achData.ach_ID))
        {
            achievementPrefab.ShowClaimButton(achData);
        }
    }

    #endregion

}
