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

    [Header("Achievement UI")]
    public GameObject m_achievementPanel;
    public GameObject m_achievementPrefab;

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
        DudeController.instance.dudeIsKilledEvent += UpdateGameOverUI;

        for (int i = 0; i < AchievementManager.instance.m_achievements.Count; i++)
        {
            CreateAchievement(m_achievementPanel, AchievementManager.instance.m_achievements[i]);
        }
    }

    public void UpdateGameOverUI()
    {
        m_sessionHS.text = GameManager.instance.GetSessionScores().ToString();
        m_sessionCP.text = GameManager.instance.GetSessionCheckpoints().ToString();
        m_allTimeHS.text = PlayerDataManager.instance.GetAllTimeHS().ToString();
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
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
        achievementPrefab.SetTitle(achData.ach_Title);
        achievementPrefab.SetDescription(achData.ach_Description, achData);
        achievementPrefab.SetCoin(achData.ach_Reward);

        if (achData.ach_Complete)
        {
            achievementPrefab.ShowCoin(true);
        }
        else
        {
            achievementPrefab.ShowCoin(false);
        }
    }

}
