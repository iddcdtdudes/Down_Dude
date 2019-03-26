using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]private Text m_Title;
    [SerializeField]private Text m_Description;
    [SerializeField]private Text m_Coin;

    [SerializeField] private GameObject m_CoinBtn;

    public void SetTitle (string title)
    {
        m_Title.text = title;
    }

    public void SetDescription (string description, Achievement achData)
    {
        m_Description.text = description + " " + achData.ach_Trigger[0].ach_Progress + "/" + achData.ach_Trigger[0].ach_Goal;
    }

    public void SetCoin (int amount)
    {
        m_Coin.text = amount.ToString();
    }

    public void ShowCoin (bool i)
    {
        Button coin = m_CoinBtn.GetComponent<Button>();
        if (i)
        {
            coin.interactable = true;
        }
        else
        {
            coin.interactable = false;
        }

    }

}
