using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "skin_")]
public class SkinObject : ScriptableObject
{

    public int m_id;
    public string m_name;
    public int m_cost;
    //public bool m_unlock;
    public AnimatorOverrideController m_skinIDAnimator;

    //UI
    public Sprite m_skinEx;
    public Sprite m_skinIcon;
    
    public int GetSkinID ()
    {
        return m_id;
    }

    public string GetSkinName ()
    {
        return m_name;
    }

    public int GetSkinCost ()
    {
        return m_cost;
    }

    public Sprite GetSkinEx ()
    {
        return m_skinEx;
    }

    public Sprite GetSkinICON ()
    {
        return m_skinIcon;
    }
}
