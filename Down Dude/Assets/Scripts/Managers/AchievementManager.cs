using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TRACKER
{SCORE, CHECKPOINT}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [SerializeField]public List<Achievement> m_achievements;

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
        //Check through each achievement
        for (int i = 0; i < m_achievements.Count; i++)
        {
            //Check if achievement have been completed
            if (m_achievements[i].ach_Complete == false)
            {
                //Update the progress and complete the achievement if goal is met
                UpdateAchProgress(m_achievements[i]);
            }
        }

    }


    #region Private
    //For pulling out achievement that are done
    private void PullAch (int achID)
    {
        m_achievements.RemoveAt(achID);
    }

    //Call in update?
    private void UpdateAchProgress (Achievement achToCheck)
    {
        for (int i = 0; i < achToCheck.ach_Trigger.Length; i++)
        {

            //Update each progress
            if (achToCheck.ach_Dynamic == true)
            {
                switch (achToCheck.ach_Trigger[i].ach_Type)
                {
                    case TRACKER.CHECKPOINT:
                        achToCheck.ach_Trigger[i].ach_Progress = PlayerDataManager.instance.GetAllTimeHS();
                        break;
                    case TRACKER.SCORE:
                        achToCheck.ach_Trigger[i].ach_Progress = PlayerDataManager.instance.GetAllTimeCP();
                        break;
                }
            }
            else
            {
                switch (achToCheck.ach_Trigger[i].ach_Type)
                {
                    case TRACKER.CHECKPOINT:
                        achToCheck.ach_Trigger[i].ach_Progress = GameManager.instance.GetSessionCheckpoints();
                        break;
                    case TRACKER.SCORE:
                        achToCheck.ach_Trigger[i].ach_Progress = GameManager.instance.GetSessionScores();
                        break;
                }
            }

            //Check if progress is done
            if (achToCheck.ach_Trigger[i].ach_Progress == achToCheck.ach_Trigger[i].ach_Goal)
            {
                achToCheck.ach_Trigger[i].ach_Doned = true;

                //Check if achievement is complete
                if (AchCheckComplete(achToCheck))
                {
                    achToCheck.ach_Complete = true;
                }
            }
            else
            {
                Debug.Log("Progress is not met with goal");
            }
        }
    }

    //For checking all the trigger in achievement
    private bool AchCheckComplete (Achievement achToCheck)
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
            if (m_achievements[i].ach_Dynamic == true)
            {
                //---------------------------------Dynamic Achievement--------------------------------------------------//
                //Check if the dynamic achievement is completed
                if (m_achievements[i].ach_Complete == true)
                {
                    //Reset the dynamic achievement
                    m_achievements[i].ach_Complete = false;
                    for (int j = 0; j < m_achievements[i].ach_Trigger.Length; j++)
                    {
                        m_achievements[i].ach_Trigger[j].ach_Doned = false;
                        switch (m_achievements[i].ach_Trigger[j].ach_Type)
                        {
                            case TRACKER.SCORE:
                                m_achievements[i].ach_Trigger[j].ach_Goal = PlayerDataManager.instance.GetAllTimeHS();
                                break;
                            case TRACKER.CHECKPOINT:
                                m_achievements[i].ach_Trigger[j].ach_Goal = PlayerDataManager.instance.GetAllTimeCP();
                                break;
                        }
                    }
                }

            }
            ////---------------------------------Static Achievement--------------------------------------------------//
            //else
            //{
            //    if (m_achievements[i].ach_Complete == false)
            //    {
            //        for (int j = 0; j < m_achievements[i].ach_Trigger.Length; j++)
            //        {
            //            m_achievements[i].ach_Trigger[j].
            //        }
            //    }
            //}
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
