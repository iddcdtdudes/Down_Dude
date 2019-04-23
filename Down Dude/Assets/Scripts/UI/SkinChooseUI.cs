using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChooseUI : MonoBehaviour
{
    public SkinUI m_previousSkin;
    public SkinUI m_currentSkin;

    public void SetSkinUI (SkinUI i)
    {
        m_previousSkin = m_currentSkin;
        m_currentSkin = i;
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
