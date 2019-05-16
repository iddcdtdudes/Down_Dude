using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverUI : MonoBehaviour
{
    public Text m_sessionDist;
    public Text m_allTimeDist;
    public Text m_sessionCP;
    public Text m_allTimeCP;
    public Text m_sessionCoins;
    public GameObject m_achievement;
    public Animator m_animGameoverUI;
    //For updating UI
    private bool m_updateGameOverUI;
    //Distance
    private float m_originalDist;
    private float m_currentDist;
    private float m_targetDist;
    //Checkpoint
    private float m_originalCP;
    private float m_currentCP;
    private float m_targetCP;
    //Coins
    private float m_originalCoins;
    private float m_currentCoins;
    private float m_targetCoins;

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
                    AudioManager.instance.Play("Score");
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
                    AudioManager.instance.Play("Score");
                    if (m_currentCP >= m_targetCP)
                    {
                        m_currentCP = m_targetCP;
                        m_gameoverUIUpdate = GameOverUIUpdate.COIN;
                    }
                }
                else
                {
                    m_gameoverUIUpdate = GameOverUIUpdate.COIN;
                }

                m_sessionCP.text = ((int)m_currentCP).ToString();
                break;
            case GameOverUIUpdate.COIN:
                if (m_currentCoins < m_targetCoins)
                {
                    m_currentCoins += (1.5f * Time.deltaTime) * (m_targetCoins - m_originalCoins);
                    AudioManager.instance.Play("Collect Coin");
                    if (m_currentCoins >= m_targetCoins)
                    {
                        m_currentCoins = m_targetCoins;
                        m_gameoverUIUpdate = GameOverUIUpdate.MENU;
                    }
                }
                else
                {
                    m_gameoverUIUpdate = GameOverUIUpdate.MENU;
                }

                m_sessionCoins.text = ((int)m_currentCoins).ToString();
                break;
            case GameOverUIUpdate.MENU:
                if (m_achievement.activeSelf)
                {
                    ShowContinueButton();
                }
                else
                {
                    ShowMenuButton();
                }
                //m_animGameoverUI.SetTrigger("UpdateScoreDone");
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
        m_originalCoins = 0f;
        //Current
        m_currentDist = m_originalDist;
        m_currentCP = m_originalCP;
        m_currentCoins = m_originalCoins;
        //Target
        m_targetDist = GameManager.instance.GetSessionDistance();
        m_targetCP = GameManager.instance.GetSessionCheckpoints();
        m_targetCoins = GameManager.instance.GetSessionCoins();

        m_sessionDist.text = m_originalDist.ToString();
        m_sessionCP.text = m_originalCP.ToString();
        m_sessionCoins.text = m_originalCoins.ToString();

        //All Time
        m_allTimeDist.text = PlayerDataManager.instance.GetAllTimeDist().ToString() + 'm';
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
    }


    private void ShowContinueButton ()
    {
        m_animGameoverUI.SetTrigger("GameoverUICont");
    }

    private void ShowMenuButton()
    {
        m_animGameoverUI.SetTrigger("GameoverUIMenu");
    }

    public void ShowGameoverUIAch ()
    {
        m_animGameoverUI.SetTrigger("GameoverUIAch");
    }
}
