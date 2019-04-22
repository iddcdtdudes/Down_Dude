using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinUI : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_icon;

    public void SetICON (Sprite icon)
    {
        m_icon.sprite = icon;
    }

    public Button GetButton ()
    {
        return m_button;
    }
    
}
