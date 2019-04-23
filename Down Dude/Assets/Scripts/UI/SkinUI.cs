using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinUI : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_icon;
    [SerializeField] private Image m_label;

    public void SetICON (Sprite icon)
    {
        m_icon.sprite = icon;
    }

    public void SetLabel (Sprite label)
    {
        m_label.sprite = label;
    }

    public void ShowLabel ()
    {
        m_label.gameObject.SetActive(true);
    }

    public void HideLabel ()
    {
        m_label.gameObject.SetActive(false);
    }

    public Button GetButton ()
    {
        return m_button;
    }
    
}
