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

    private int CoinValue;

    public void SetTitle (string title)
    {
        m_Title.text = title;
    }

    public void SetDescription (string description)
    {
        m_Description.text = description;
    }

    public void SetCoin (int amount)
    {
        m_Coin.text = amount.ToString();
        Button coin = m_CoinBtn.GetComponent<Button>();
        coin.interactable = false;
    }

    public void ShowCoin (int reward, Achievement achData)
    {
        Button coin = m_CoinBtn.GetComponent<Button>();
        if (achData.ach_rewardClaimed)
        {
            coin.gameObject.SetActive(false);
        }
        else
        {
            if (achData.ach_object.ach_Dynamic)
            {
                coin.onClick.AddListener(delegate { PlayerDataManager.instance.AddCoins(reward); AudioManager.instance.Play("Button"); AchievementManager.instance.ResetAchievement(achData); HideCoin(coin); PlayerDataManager.instance.SaveDataLocal(); });
            }
            else
            {
                coin.onClick.AddListener(delegate { PlayerDataManager.instance.AddCoins(reward); AudioManager.instance.Play("Button"); HideCoin(coin); PlayerDataManager.instance.SaveDataLocal();  });
            }
            coin.interactable = true;
        }
        
        
        
    }


    public void HideCoin (Button coin)
    {
        coin.interactable = false;
    }

}
