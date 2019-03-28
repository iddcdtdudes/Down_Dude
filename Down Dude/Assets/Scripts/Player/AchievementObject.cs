using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ach_")]
public class AchievementObject : ScriptableObject
{
    //For UI
    public int ach_ID;
    public int ach_Reward;
    public string ach_Title;
    public string ach_Description;
    public Sprite ach_icon;

    //For checking conditions of the achievement
    public TriggerTracker[] ach_Trigger;
    public bool ach_Dynamic;

    public int GetTriggerSize ()
    {
        return ach_Trigger.Length;
    }

    public void SetTrigger (int index)
    {
        if (index > ach_Trigger.Length || index < 0)
        {
            Debug.Log("Trigger Index Error");
        }
        else
        {
            ach_Trigger[index].ach_Doned = true;
        }
    }

    public void ResetTrigger (int index)
    {
        if (index > ach_Trigger.Length || index < 0)
        {
            Debug.Log("Trigger Index Error");
        }
        else
        {
            ach_Trigger[index].ResetTrigger();
        }
    }

    public TRACKER GetTriggerType(int index)
    {
        return ach_Trigger[index].ach_Type;
    }

}
