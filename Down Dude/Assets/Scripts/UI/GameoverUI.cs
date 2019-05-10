using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] public Text m_sessionDist;
    public Text m_allTimeDist;
    public Text m_sessionCP;
    public Text m_allTimeCP;
    [SerializeField] private Animator m_animGameoverUI;
    //For updating UI
    private bool m_updateGameOverUI;
    private float m_originalDist;
    private float m_currentDist;
    private float m_targetDist;
    private float m_originalCP;
    private float m_currentCP;
    private float m_targetCP;
    private GameOverUIUpdate m_gameoverUIUpdate;
    //UI
   

    private void Start()
    {

    }

    private void Update()
    {
        if (m_updateGameOverUI)
        {
            UpdateGameOverUI();
        }
    }

    private void UpdateGameOverUI()
    {
        switch (m_gameoverUIUpdate)
        {
            case GameOverUIUpdate.DISTANCE:
                if (m_currentDist < m_targetDist)
                {
                    //Debug.Log("Update Distance");
                    m_currentDist += (1.5f * Time.deltaTime) * (m_targetDist - m_originalDist);
                    if (m_currentDist >= m_targetDist)
                    {
                        m_currentDist = m_targetDist;
                        m_gameoverUIUpdate = GameOverUIUpdate.CHECKPOINT;
                    }
                }
                else
                {
                    m_gameoverUIUpdate = GameOverUIUpdate.CHECKPOINT;
                }

                m_sessionDist.text = ((int)m_currentDist).ToString();
                break;
            case GameOverUIUpdate.CHECKPOINT:

                if (m_currentCP < m_targetCP)
                {
                    //Debug.Log("Update CP");
                    m_currentCP += (1.5f * Time.deltaTime) * (m_targetCP - m_originalCP);
                    if (m_currentCP >= m_targetCP)
                    {
                        m_currentCP = m_targetCP;
                        m_gameoverUIUpdate = GameOverUIUpdate.MENU;
                    }
                }
                else
                {
                    m_gameoverUIUpdate = GameOverUIUpdate.MENU;
                }

                m_sessionCP.text = ((int)m_currentCP).ToString();
                break;
            case GameOverUIUpdate.MENU:
                
                m_animGameoverUI.SetTrigger("UpdateScoreDone");
                break;
        }
        //m_sessionDist.text = ((int)GameManager.instance.GetSessionDistance()).ToString();


    }

    public void StartUpdateScore()
    {
        m_updateGameOverUI = true;
        m_gameoverUIUpdate = GameOverUIUpdate.DISTANCE;
        //Origin
        m_originalDist = 0f;
        m_originalCP = 0f;
        //Current
        m_currentDist = m_originalDist;
        m_currentCP = m_originalCP;
        //Target
        m_targetDist = GameManager.instance.GetSessionDistance();
        m_targetCP = GameManager.instance.GetSessionCheckpoints();

        m_sessionDist.text = m_originalDist.ToString();
        m_sessionCP.text = m_originalCP.ToString();

        //All Time
        m_allTimeDist.text = PlayerDataManager.instance.GetAllTimeDist().ToString();
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
    }

}
