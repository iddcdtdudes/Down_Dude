using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField]private int m_currentSkin;

    private int m_skinsTotal;

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
        m_skinsTotal = m_skins.Length;
    }

    private void Update()
    {
        ChangeSkin(m_currentSkin);
    }

    void ChangeSkin (int skinID)
    {
        //Change animator controller according to input skin id
        if (m_currentSkin != skinID)
        {
            m_currentSkin = skinID;

            if (m_dudeAnimator != null)
            {
                m_dudeAnimator.runtimeAnimatorController = m_skins[m_currentSkin].m_skinIDAnimator;
            }
            else
            {
                Debug.Log("Cant find animator");
            }
        }
        
    }

    public int GetSkinsNumber ()
    {
        return m_skinsTotal;
    }
}
