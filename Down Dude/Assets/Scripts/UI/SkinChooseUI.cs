using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChooseUI : MonoBehaviour
{
    public SkinUI m_previousSkin;
    public SkinUI m_currentSkin;

    public void SetSkinUI (SkinUI i)
    {
        if (m_previousSkin == null)
        {
            m_previousSkin = i;
        }
        else
        {
            m_previousSkin = m_currentSkin;
        }
        m_currentSkin = i;
    }

    public void HidePreviousLabel ()
    {
        m_previousSkin.HideLabel();
    }

    public void HideCurrentLabel ()
    {
        m_currentSkin.HideLabel();
    }

    public void ShowCurrentLabel ()
    {
        m_currentSkin.ShowLabel();
    }

    public SkinUI GetCurrSkinUI ()
    {
        return m_currentSkin;
    }

    public SkinUI GetPrevSkinUI ()
    {
        return m_previousSkin;
    }

}
