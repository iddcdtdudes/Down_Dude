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
    }

    public void ShowCoin (bool i, int reward)
    {
        Button coin = m_CoinBtn.GetComponent<Button>();
        coin.onClick.AddListener(delegate { PlayerDataManager.instance.AddCoins(reward);  });
        coin.interactable = true;
    }

}
