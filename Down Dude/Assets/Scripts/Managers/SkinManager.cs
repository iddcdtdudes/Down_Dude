using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField]private int m_currentSkin;

    //[SerializeField]public int m_skinsTotal;

    [SerializeField] public Skin[] m_skins;

    [SerializeField] private SkinObject[] m_skin;

    [SerializeField]private Animator m_dudeAnimator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //m_dudeAnimator = DudeController.instance.GetComponentInChildren<Animator>();
        m_currentSkin = PlayerDataManager.instance.GetUsingSkin();
        //OverrideAnimator(m_currentSkin);
        UIManager.instance.m_skinExample.sprite = m_skin[m_currentSkin].GetSkinEx();
        Debug.Log("Current skin = " + m_currentSkin);
    }

    private void OverrideAnimator (int skinID)
    {
        //if (m_currentSkin != m_chooseSkin)
        //{
            if (m_dudeAnimator != null)
            {   
                //m_dudeAnimator.runtimeAnimatorController = m_skins[skinID].m_skinIDAnimator;
                m_dudeAnimator.runtimeAnimatorController = m_skin[skinID].m_skinIDAnimator;
            }
            else
            {
                Debug.Log("Cant find animator");
            }
        //}
       // else
        //{
           // Debug.Log("Same skin");
        //}
    }

    //For changing in UI
    public void ChangeSkin (int skinID)
    {
        //Change animator controller according to input skin id
        if (m_currentSkin != skinID)
        {
            if (PlayerDataManager.instance.GetSkin(skinID))
            {
                m_currentSkin = skinID;
                PlayerDataManager.instance.SetUsingSkin(m_currentSkin);
                OverrideAnimator(m_currentSkin);
            }
        }
        else
        {
            m_currentSkin = 0;
            PlayerDataManager.instance.SetUsingSkin(0);
            Debug.Log("Skin is locked");
        }
    }

    public int GetSkinsNumber ()
    {
        return m_skin.Length;
    }

    public SkinObject GetSkin (int skinID)
    {
        return m_skin[skinID];
    }

    public void BuySkin (int skinID)
    {
        int skinCost = m_skin[skinID].GetSkinCost();

        if (!PlayerDataManager.instance.GetSkin(skinID))
        {
            if (PlayerDataManager.instance.GetCoin() >= skinCost)
            {
                PlayerDataManager.instance.SubtractCoins(skinCost);
                PlayerDataManager.instance.SetUnlockSkin(skinID);
            }
        }
    }
}
