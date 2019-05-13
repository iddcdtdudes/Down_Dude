using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [Header("Game Over UI")]
    //For updating UI
    public Animator m_animGameOverUI;
    //public Text m_achievementCompleted;
    public GameObject m_achGameOverPanel;
    public GameObject m_achGameOverPrefab;
    public GameObject m_achGameOver;

    [Header("In-Game Panel")]
    public GameObject m_inGamePanel;

    [Header("Achievement UI")]
    public GameObject m_achNotif;
    public Text m_achNotifNum;
    public GameObject m_achievementPanel;
    public GameObject m_achievementPrefab;

    [Header("Skin UI")]
    //Middle
    public Text m_skinName;
    public Text m_skinCost;
    public GameObject m_skinBuyButton;
    public GameObject m_skinSelectButton;
    public Image m_skinExample;
    //Lower
    public GameObject m_skinPanel;
    public GameObject m_skinPrefab;
    //Sprite
    public Sprite m_lockedLabel;
    public Sprite m_selectLabel;

    [Header("Stat UI")]
    public Text m_death;
    public Text m_statCP;
    public Text m_statDistance;

    [Header("Menu")]
    public GameObject m_menuPanel;
    public Text m_coins;
    public GameObject m_startText;
    public Animator m_menuAnim;
    public GameObject m_musicOnButton;
    public GameObject m_musicOffButton;

    [Header("Settings")]
    public Text m_dudeControlButton;
    public Text m_dudeControlTouch;

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

        //m_updateGameOverUI = false;

        //Update Coins in menu
        UpdateCoinValue();
        //DudeControlChange();
        //Create each achievement list in UI

    }

    private void Update()
    {
        //if (m_updateGameOverUI)
        //{
        //    UpdateGameOverUI();
        //}
    }

    #region Main Menu

    public void UpdateCoinValue ()
    {
        //Update Coins in menu
        m_coins.text = PlayerDataManager.instance.GetCoin().ToString();
    }

    public void OnButtonPressed ()
    {
        AudioManager.instance.Play("Ok");
    }

    public void ShowMusicSetting()
    {
        if (PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Music))
        {
            if (PlayerPrefs.GetInt(PlayerDataManager.instance.m_playerPref_Music) == 1)
            {
                m_musicOffButton.SetActive(true);
                m_musicOnButton.SetActive(false);
            }
            else
            {
                m_musicOffButton.SetActive(false);
                m_musicOnButton.SetActive(true);
            }
        }
        else
        {
            Debug.Log("No music key");
        }
    }

    public void SetMusic (bool i)
    {
        AudioManager.instance.Music(i);
    }

    public void ToggleMenu (bool i)
    {
        m_menuAnim.SetBool("ButtomMenuToggle", i);
    }

    public void HideMenu ()
    {
        m_startText.SetActive(false);
        //m_menuAnim.SetBool("GameStart", true);
        m_menuAnim.SetTrigger("GameStart");
        //m_menuPanel.SetActive(false);
        m_inGamePanel.SetActive(true);
        DudeController.instance.ShowButtonUI();
        //m_inGamePanel.GetComponent<Animator>().SetBool("game_start", true);
    }
    #endregion

    #region Gameover UI

    public void StartGameOverUI ()
    {
        m_animGameOverUI.SetTrigger("UpdateScoreStart");
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
            if (!PlayerDataManager.instance.GetAchievementClaimed(i))
                CreateAchievement(m_achievementPanel, AchievementManager.instance.m_achievements[i]);
        }
    }

    private void CreateAchievement (GameObject achParent, AchievementObject achData)
    {
        GameObject achPrefab = Instantiate(m_achievementPrefab);

        SetAchievementInfo(achParent, achPrefab , achData);

        //Debug.Log("Create achievement in menu");
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

    #region Skin Panel

    public void SelectSkin (int skinID, SkinUI prefab)
    {
        //Set Skin Name
        m_skinName.text = SkinManager.instance.GetSkin(skinID).GetSkinName();
        m_skinExample.sprite = SkinManager.instance.GetSkin(skinID).GetSkinEx();
        //Reset Click Function
        
        
        if (PlayerDataManager.instance.GetSkin(skinID))
        {
            //Get Component From Select Button
            SkinChooseUI skinSelectButton = m_skinSelectButton.GetComponent<SkinChooseUI>();
            //Reset Select Button
            m_skinSelectButton.GetComponent<Button>().onClick.RemoveAllListeners();
            //Send Data to select button
            skinSelectButton.HoldSkinUI(prefab);
            //Show Select Button
            if (skinSelectButton.GetCurrSkinUI().GetSkinID() != skinSelectButton.GetTmpSkinUI().GetSkinID())
            {
                //Show Select Button
                ShowSelectButton();
                //Set Select Button
                m_skinSelectButton.GetComponent<Button>().onClick.AddListener(delegate
                {
                    SkinManager.instance.ChangeSkin(skinID);
                    //PlayerDataManager.instance.SetUsingSkin(skinID);
                    prefab.SetLabel(m_selectLabel);
                    skinSelectButton.SetSkinUI();
                    skinSelectButton.GetCurrSkinUI().SetLabel(m_selectLabel);
                    skinSelectButton.ShowCurrentLabel();
                    skinSelectButton.HidePreviousLabel();
                    OnButtonPressed();
                    m_skinSelectButton.SetActive(false);

                });
            }
            else
            {
                m_skinSelectButton.SetActive(false);
                m_skinBuyButton.SetActive(false);

            }
            
            
            
        }
        else
        {
            //Reset Buy Button
            m_skinBuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
            //Show Buy Button
            ShowBuyButton();
            m_skinCost.text = SkinManager.instance.GetSkin(skinID).GetSkinCost().ToString();
            //Set Select Button
            m_skinBuyButton.GetComponent<Button>().onClick.AddListener(delegate
            {
                //Unlock Skin
                SkinManager.instance.BuySkin(skinID, prefab);
                
            });
        }
    }

    public void CreateSkinMenu ()
    {
        for (int i = 0; i < SkinManager.instance.GetSkinsNumber(); i++)
        {
            CreateSkin(m_skinPanel, SkinManager.instance.GetSkin(i));
        }

        m_skinExample.sprite = SkinManager.instance.GetSkin(PlayerDataManager.instance.GetUsingSkin()).GetSkinEx();
        m_skinName.text = SkinManager.instance.GetSkin(PlayerDataManager.instance.GetUsingSkin()).GetSkinName();
    }

    private void CreateSkin (GameObject skinParent, SkinObject skinData)
    {
        GameObject skinPrefab = Instantiate(m_skinPrefab);

        SetSkinInfo(skinParent, skinPrefab, skinData);
    }

    private void SetSkinInfo (GameObject skinParent, GameObject skinPrefab, SkinObject skinData)
    {
        skinPrefab.transform.SetParent(skinParent.transform);
        skinPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        SkinUI ui = skinPrefab.GetComponent<SkinUI>();
        ui.SetICON(skinData.GetSkinICON());
        ui.SetSkinID(skinData.GetSkinID());

        if (PlayerDataManager.instance.GetUsingSkin() == skinData.GetSkinID())
        {
            ui.SetLabel(m_selectLabel);
            m_skinSelectButton.GetComponent<SkinChooseUI>().HoldSkinUI(ui);
            m_skinSelectButton.GetComponent<SkinChooseUI>().SetSkinUI();

        }
        else
        {
            if (!PlayerDataManager.instance.GetSkin(skinData.GetSkinID()))
            {
                ui.SetLabel(m_lockedLabel);
            }
            else
            {
                ui.HideLabel();
            }
        }

        ui.GetButton().onClick.AddListener(delegate
        {
            SelectSkin(skinData.GetSkinID(), ui);
        });
    }

    public void ShowSelectButton ()
    {
        m_skinSelectButton.SetActive(true);
        m_skinBuyButton.SetActive(false);
    }

    public void ShowBuyButton ()
    {
        m_skinSelectButton.SetActive(false);
        m_skinBuyButton.SetActive(true);
    }

    public void HideInGamePanel ()
    {
        m_inGamePanel.SetActive(false);
    }

    #endregion

    #region Statistics

    public void UpdateStatPage ()
    {
        m_death.text = PlayerDataManager.instance.GetDeath().ToString();
        m_statCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
        m_statDistance.text = PlayerDataManager.instance.GetAllTimeDist().ToString();
    }

    #endregion

    public void DudeControlChange()
    {
        //if (i >= 0.5)
        //{
        //    m_dudeControlTouch.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        //    m_dudeControlButton.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        //}
        //else
        //{
        //    m_dudeControlTouch.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        //    m_dudeControlButton.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        //}

        if (PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Control))
        {
            Slider controlSetting = m_dudeControlTouch.GetComponentInParent<Slider>();
            controlSetting.onValueChanged.AddListener(delegate
            {
                AudioManager.instance.Play("Ok");
            });

            if (PlayerPrefs.GetInt(PlayerDataManager.instance.m_playerPref_Control) == 1)
            {
                controlSetting.value = 1;
                m_dudeControlTouch.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
                m_dudeControlButton.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                controlSetting.value = 0;
                m_dudeControlTouch.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                m_dudeControlButton.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
            }
        }
    }
}

enum GameOverUIUpdate
{ DISTANCE, CHECKPOINT, MENU}
