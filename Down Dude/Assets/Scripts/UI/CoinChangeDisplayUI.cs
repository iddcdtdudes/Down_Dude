using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinChangeDisplayUI : MonoBehaviour
{
    [SerializeField] private Text m_coinChangeText;

    public void PlusCoinAmount(int amount)
    {
        m_coinChangeText.text = (amount >= 0 ? "+" + amount.ToString() : amount.ToString());
    }

    public void MinusCoinAmount(int amount)
    {
        m_coinChangeText.text = (amount >= 0 ? "-" + amount.ToString() : amount.ToString());
    }

    private void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
