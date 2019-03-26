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



}
