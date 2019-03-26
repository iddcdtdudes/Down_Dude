using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField]private int m_currentSkin;

    private int m_chooseSkin;

    //[SerializeField]public int m_skinsTotal;

    [SerializeField] private Skin[] m_skins;

    private Animator m_dudeAnimator;

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
        m_dudeAnimator = DudeController.instance.GetComponentInChildren<Animator>();
        m_currentSkin = PlayerDataManager.instance.GetUsingSkin();

        ChangeSkin(m_currentSkin);
        //OverrideAnimator(m_currentSkin);

        Debug.Log("Current skin = " + m_currentSkin);
    }

    private void LateUpdate()
    {
        //OverrideAnimator();
    }

    private void OverrideAnimator (int skinID)
    {
        //if (m_currentSkin != m_chooseSkin)
        //{
            if (m_dudeAnimator != null)
            {   
                m_dudeAnimator.runtimeAnimatorController = m_skins[skinID].m_skinIDAnimator;
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
                    PlayerDataManager.instance.SetUsingSkin(m_chooseSkin);

                //m_chooseSkin = skinID;
                //PlayerDataManager.instance.SetUsingSkin(m_currentSkin);
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
        return m_skins.Length;
    }
}
