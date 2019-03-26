using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public AchievementObject ach_object;

    public bool ach_complete;

    public int GetReward()
    {
        return ach_object.ach_Reward;
    }

    public void SetComplete ()
    {
        ach_complete = true;
    }

    public void ResetComplete ()
    {
        ach_complete = false;
    }

    public bool GetComplete ()
    {
        return ach_complete;
    }
}
