using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRACKER
{SCORE, CHECKPOINT}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [SerializeField]public List<Achievement> m_achievements;

    public event Action achiCompleteEvent;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ////Check through each achievement
        //for (int i = 0; i < m_achievements.Count; i++)
        //{
        //    //Check if achievement have been completed
        //    if (m_achievements[i].ach_Complete == false)
        //    {
        //        //Update the progress and complete the achievement if goal is met
        //        UpdateAchProgress(m_achievements[i]);
        //    }
        //}

    }


    #region Private
    //For pulling out achievement that are done
    private void PullAch (int achID)
    {
        m_achievements.RemoveAt(achID);
    }

    //Call in update?
    public void UpdateAchProgress ()
    {
        for (int i = 0; i < m_achievements.Count; i++)
        {
            if (m_achievements[i].GetComplete() == false)
            {
                for (int j = 0; j < m_achievements[i].ach_object.ach_Trigger.Length; j++)
                {
                    switch (m_achievements[i].ach_object.ach_Trigger[j].ach_Type)
                    {
                        case TRACKER.CHECKPOINT:
                            m_achievements[i].ach_object.ach_Trigger[j].ach_Progress = GameManager.instance.GetSessionCheckpoints();
                            break;
                        case TRACKER.SCORE:
                            m_achievements[i].ach_object.ach_Trigger[j].ach_Progress = GameManager.instance.GetSessionScores();
                            break;
                    }

                    //Check if progress is done
                    if (m_achievements[i].ach_object.ach_Trigger[j].ach_Progress > m_achievements[i].ach_object.ach_Trigger[j].ach_Goal)
                    {
                        m_achievements[i].ach_object.ach_Trigger[j].ach_Doned = true;

                        //Check if achievement is complete
                        if (AchCheckComplete(m_achievements[i].ach_object))
                        {
                            m_achievements[i].SetComplete();
                            PlayerDataManager.instance.SetUnlockAch(i);

                            //Update achievement UI in Gameover UI
                            UIManager.instance.UpdateAchiUI(m_achievements[i].ach_object);

                            if (achiCompleteEvent != null)
                            {
                                achiCompleteEvent.Invoke();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Progress is not met with goal");
                    }
                }
            }
        }
    }

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

    //For resetting the dynamic achievement
    public void ResetAchievement()
    {
        //Reset Dynamic Achievement
        for (int i = 0; i < m_achievements.Count; i++)
        {
            //Check if the achievement is dynamic
            if (m_achievements[i].ach_object.ach_Dynamic == true)
            {
                //---------------------------------Dynamic Achievement--------------------------------------------------//
                //Check if the dynamic achievement is completed
                if (m_achievements[i].GetComplete())
                {
                    //Reset the dynamic achievement
                    m_achievements[i].ResetComplete();
                    for (int j = 0; j < m_achievements[i].ach_object.ach_Trigger.Length; j++)
                    {
                        m_achievements[i].ach_object.ach_Trigger[j].ach_Doned = false;
                        switch (m_achievements[i].ach_object.ach_Trigger[j].ach_Type)
                        {
                            case TRACKER.SCORE:
                                m_achievements[i].ach_object.ach_Trigger[j].ach_Goal = PlayerDataManager.instance.GetAllTimeHS();
                                break;
                            case TRACKER.CHECKPOINT:
                                m_achievements[i].ach_object.ach_Trigger[j].ach_Goal = PlayerDataManager.instance.GetAllTimeCP();
                                break;
                        }
                    }
                }

            }
            ////---------------------------------Static Achievement--------------------------------------------------//
            else
            {
                if (m_achievements[i].GetComplete() == false)
                {
                    for (int j = 0; j < m_achievements[i].ach_object.ach_Trigger.Length; j++)
                    {
                        m_achievements[i].ach_object.ach_Trigger[j].ach_Doned = false;
                        m_achievements[i].ach_object.ach_Trigger[j].ach_Progress = 0;
                    }
                }
            }
        }

    }


}

[System.Serializable]
public class TriggerTracker
{
    public TRACKER ach_Type;
    public int ach_Goal;
    public int ach_Progress;
    public bool ach_Doned;
}
