﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]private Text m_Title;
    [SerializeField]private Text m_Description;
    [SerializeField]private Text m_reward;

    [SerializeField] private GameObject m_claimButton;

    public void SetTitle (string title)
    {
        m_Title.text = title;
    }

    public void SetDescription (string description)
    {
        m_Description.text = description;
    }

    public void SetReward (int amount)
    {
        m_reward.text = amount.ToString();
        Button claimButton = m_claimButton.GetComponent<Button>();
        claimButton.interactable = false;
    }

    public void ShowClaimButton (AchievementObject achData)
    {
        Button claimButton = m_claimButton.GetComponent<Button>();

        //If achievement is claimed
        //Hide coin button
        if (PlayerDataManager.instance.GetAchievementClaimed( achData.ach_ID ) )
        {
            claimButton.gameObject.SetActive(false);
        }
        else
        {
            if (achData.ach_Dynamic)
            {
                claimButton.onClick.AddListener(delegate 
                {
                    //Add coins
                    PlayerDataManager.instance.AddCoins(achData.ach_Reward);
                    //Play Sound
                    AudioManager.instance.Play("Ok");
                    //Reset achievement
                    PlayerDataManager.instance.ResetUnlockAch(achData.ach_ID);
                    //Hide claim button
                    HideClaimButton(claimButton);
                    //Update coin in UI
                    UIManager.instance.UpdateCoinValue();
                    //Save player data
                    PlayerDataManager.instance.SaveDataLocal();
                });
            }
            else
            {
                claimButton.onClick.AddListener(delegate 
                {
                    //Add coins
                    PlayerDataManager.instance.AddCoins(achData.ach_Reward);
                    //Play sound
                    AudioManager.instance.Play("Ok");
                    HideClaimButton(claimButton);
                    //Update coin in UI
                    UIManager.instance.UpdateCoinValue();
                    //Set achievement as claimed
                    PlayerDataManager.instance.SetAchievementClaimed(achData.ach_ID);
                    claimButton.gameObject.SetActive(false);
                    //Save Data
                    PlayerDataManager.instance.SaveDataLocal();
                });
            }

            //Show Button
            claimButton.interactable = true;
        }
    }

    public void HideClaimButton (Button claimButton)
    {
        claimButton.interactable = false;
    }

}
