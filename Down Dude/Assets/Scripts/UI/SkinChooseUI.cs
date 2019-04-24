using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChooseUI : MonoBehaviour
{
    public SkinUI m_previousSkin;
    public SkinUI m_tmpSkin;
    public SkinUI m_currentSkin;

    #region Setter

    public void SetSkinUI ()
    {
        if (m_previousSkin == null)
        {
            m_previousSkin = m_tmpSkin;
        }
        else
        {
            m_previousSkin = m_currentSkin;
        }

        m_currentSkin = m_tmpSkin;
    }

    public void HoldSkinUI (SkinUI i)
    {
        m_tmpSkin = i;
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

    #endregion

    #region Getter

    public SkinUI GetCurrSkinUI ()
    {
        return m_currentSkin;
    }

    public SkinUI GetPrevSkinUI ()
    {
        return m_previousSkin;
    }

    public SkinUI GetTmpSkinUI ()
    {
        return m_tmpSkin;
    }

    #endregion
}
