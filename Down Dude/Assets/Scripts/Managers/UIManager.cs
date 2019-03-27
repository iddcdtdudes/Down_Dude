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
    public Text m_achievementCompleted;

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

    private void Start()
    {
        //for (int i = 0; i < AchievementManager.instance.m_achievements.Count; i++)
        //{
        //    if (PlayerDataManager.instance.m_player.m_unlockedAchievements[i])
        //    {
        //        AchievementManager.instance.m_achievements[i].SetComplete();
        //    }
        //}
        AchievementManager.instance.LoadFromPlayerdata();

        //Update Coins in menu
        m_coins.text = PlayerDataManager.instance.GetCoin().ToString();

        //Create each achievement list in UI
        for (int i = 0; i < AchievementManager.instance.m_achievements.Count; i++)
        {
            CreateAchievement(m_achievementPanel, AchievementManager.instance.m_achievements[i]);
        }

        //AchievementManager.instance.ResetAchievement();
    }

    

    public void UpdateGameOverUI()
    {
        m_sessionHS.text = GameManager.instance.GetSessionScores().ToString();
        m_sessionCP.text = GameManager.instance.GetSessionCheckpoints().ToString();
        m_allTimeHS.text = PlayerDataManager.instance.GetAllTimeHS().ToString();
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
    }

    public void UpdateAchiUI (AchievementObject achiData)
    {
        m_achievementCompleted.text = achiData.ach_Title;
    }

    public void CreateAchievement (GameObject achParent, Achievement achData)
    {
        GameObject achPrefab = Instantiate(m_achievementPrefab);

        SetAchievementInfo(achParent, achPrefab , achData);
    }

    public void SetAchievementInfo (GameObject achParent, GameObject achPrefab, Achievement achData)
    {
        achPrefab.transform.SetParent(achParent.transform);
        achPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        AchievementUI achievementPrefab = achPrefab.GetComponent<AchievementUI>();
        achievementPrefab.SetTitle(achData.ach_object.ach_Title);
        achievementPrefab.SetDescription(achData.ach_object.ach_Description);
        achievementPrefab.SetCoin(achData.ach_object.ach_Reward);

        if (achData.GetComplete())
        {
            achievementPrefab.ShowCoin(achData.GetReward(), achData);
        }
        //else
        //{
        //    achievementPrefab.HideCoin();
        //}
    }

}
