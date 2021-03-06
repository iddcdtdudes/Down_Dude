﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField]private int m_currentSkin;

    //[SerializeField]public int m_skinsTotal;

    //[SerializeField] public Skin[] m_skins;

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
        //UIManager.instance.m_skinExample.sprite = m_skin[m_currentSkin].GetSkinEx();
        //UIManager.instance.m_skinName.text = m_skin[m_currentSkin].GetSkinName();
        //Debug.Log("Current skin = " + m_currentSkin);
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
                
            }
            else
            {
                Debug.Log("Skin is locked");
            }
            OverrideAnimator(m_currentSkin);
            //Save player data
            PlayerDataManager.instance.SaveDataLocal();
        }
        
        
    }

    public int GetSkinsNumber ()
    {
        return m_skin.Length;
    }

    public AnimatorOverrideController GetSkinAnimator (int skinID)
    {
        return m_skin[skinID].m_skinIDAnimator;
    }

    public SkinObject GetSkin (int skinID)
    {
        return m_skin[skinID];
    }

    public void BuySkin (int skinID, SkinUI prefab)
    {
        int skinCost = m_skin[skinID].GetSkinCost();

        if (!PlayerDataManager.instance.GetSkin(skinID))
        {
            if (PlayerDataManager.instance.GetCoin() >= skinCost)
            {
                //Player Sound
                UIManager.instance.OnButtonPressed();
                //Unlocked Skin
                PlayerDataManager.instance.SubtractCoins(skinCost);
                PlayerDataManager.instance.SetUnlockSkin(skinID);
                //
                UIManager.instance.m_skinBuyButton.SetActive(false);
                UIManager.instance.m_skinSelectButton.SetActive(true);
                UIManager.instance.UpdateCoinValue();
                //Change Label
                prefab.HideLabel();
                Button selectButton = UIManager.instance.m_skinSelectButton.GetComponent<Button>();
                //Reset Select Button
                selectButton.onClick.RemoveAllListeners();

                ////Add Function
                //selectButton.onClick.AddListener(delegate
                //{
                //    ChangeSkin(skinID);
                //    //Hide Label
                //    selectButton.GetComponent<SkinChooseUI>().SetSkinUI();
                //    selectButton.GetComponent<SkinChooseUI>().GetCurrSkinUI().SetLabel(UIManager.instance.m_selectLabel);
                //    selectButton.GetComponent<SkinChooseUI>().ShowCurrentLabel();
                //    selectButton.GetComponent<SkinChooseUI>().HidePreviousLabel();
                //    //Hide Select Button
                //    selectButton.gameObject.SetActive(false);
                //    //Play Sound
                //    UIManager.instance.OnButtonPressed();
                //});
                UIManager.instance.MinusCoinSkin(skinCost);

                //SaveData
                PlayerDataManager.instance.SaveDataLocal();

                UIManager.instance.SelectSkin(skinID, prefab);

            }
            else
            {
                AudioManager.instance.Play("Cancel");
            }

        }
        
    }
}
