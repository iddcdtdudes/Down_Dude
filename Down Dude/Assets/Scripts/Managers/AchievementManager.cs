using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRACKER
{DISTANCE, CHECKPOINT, DEATH}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [SerializeField]public List<AchievementObject> m_achievements;

    public event Action AchiCompleteEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {

    }

    #region Private

    //For checking all the trigger in achievement
    private bool AchCheckComplete (AchievementObject achToCheck)
    {
        for (int j = 0; j < achToCheck.ach_Trigger.Length; j++)
        {
            if (achToCheck.ach_Trigger[j].ach_Doned == false)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Public

    public void ResetAchievementGoal ()
    {
        //---------------------------------Dynamic Achievement--------------------------------------------------//
        //Check if the dynamic achievement is completed
        for (int i = 0; i < m_achievements.Count; i++)
        {
            if (m_achievements[i].ach_Dynamic)
            {
                //Reset the dynamic achievement Goal
                for (int j = 0; j < m_achievements[i].GetTriggerSize(); j++)
                {
                    switch (m_achievements[i].GetTriggerType(j))
                    {
                        case TRACKER.DISTANCE:
                            m_achievements[i].ach_Trigger[j].SetTriggerGoal(PlayerDataManager.instance.GetAllTimeDist());
                            break;
                        case TRACKER.CHECKPOINT:
                            m_achievements[i].ach_Trigger[j].SetTriggerGoal(PlayerDataManager.instance.GetAllTimeCP());
                            break;
                    }
                }
            }
        }
    }

    public void ResetAchProgress ()
    {
        for (int i= 0; i < m_achievements.Count; i++)
        {
            for (int j = 0; j < m_achievements[i].GetTriggerSize(); j++)
            {
                m_achievements[i].ResetTrigger(j);
            }
        }
    }

    //Call in checkPointReachedEvent
    public void UpdateAchProgress()
    {
        //Loop through achievement
        for (int i = 0; i < m_achievements.Count; i++)
        {
            //If achievement is not completed
            if (PlayerDataManager.instance.GetUnlockedAchievement(m_achievements[i].ach_ID) == false)
            {

                //Loop through all trigger
                for (int j = 0; j < m_achievements[i].ach_Trigger.Length; j++)
                {

                    //Update achievement progress
                    switch (m_achievements[i].ach_Trigger[j].ach_Type)
                    {
                        case TRACKER.CHECKPOINT:
                            m_achievements[i].ach_Trigger[j].ach_Progress = GameManager.instance.GetSessionCheckpoints();
                            if (m_achievements[i].ach_Dynamic)
                            {
                                m_achievements[i].ach_Trigger[j].ach_Goal = PlayerDataManager.instance.GetAllTimeCP();
                            }
                            break;
                        case TRACKER.DISTANCE:
                            m_achievements[i].ach_Trigger[j].ach_Progress = (int)GameManager.instance.GetSessionDistance();
                            if (m_achievements[i].ach_Dynamic)
                            {
                                m_achievements[i].ach_Trigger[j].ach_Goal = (int)PlayerDataManager.instance.GetAllTimeDist();
                            }
                            break;
                        case TRACKER.DEATH:
                            m_achievements[i].ach_Trigger[j].ach_Progress = PlayerDataManager.instance.GetDeath();
                            break;
                    }

                    //Check if progress is more than goal for that trigger
                    if (m_achievements[i].ach_Trigger[j].ach_Progress > m_achievements[i].ach_Trigger[j].ach_Goal)
                    {
                        m_achievements[i].ach_Trigger[j].ach_Doned = true;
                    }
                }
            }

            //Check through achievement list for one that all trigger is true
            if (!PlayerDataManager.instance.GetUnlockedAchievement(m_achievements[i].ach_ID))
            {
                if (AchCheckComplete(m_achievements[i]))
                {
                    //m_achievements[i].SetComplete();
                    PlayerDataManager.instance.SetUnlockAch(m_achievements[i].ach_ID);
                    UIManager.instance.m_achGameOver.SetActive(true);
                    //Create achievement UI in Gameover UI
                    UIManager.instance.CreateAchInGameOverUI(m_achievements[i].ach_Description);

                    if (AchiCompleteEvent != null)
                    {
                        AchiCompleteEvent.Invoke();
                    }
                }
            }
            
        }
    }

    #endregion
}

[System.Serializable]
public class TriggerTracker
{
    public TRACKER ach_Type;
    public int ach_Goal;
    public int ach_Progress;
    public bool ach_Doned;

    public void SetTriggerGoal(int newGoal)
    {
        ach_Goal = newGoal;
    }

    public void SetTriggerGoal(float newGoal)
    {
        ach_Goal = (int)newGoal;
    }

    public void ResetTrigger ()
    {
        ach_Progress = 0;
        ach_Doned = false;
    }
}
