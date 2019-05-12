using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstruction : MonoBehaviour
{
    [SerializeField] private string id;     // id of instruction, used to trigger display from chunk manager

    [SerializeField] private GameObject m_instructionPanel;     // instruction panel

    private void Start()
    {
        m_instructionPanel.SetActive(false);
    }

    public void Display()
    {
        m_instructionPanel.SetActive(true);
        GameManager.instance.PauseGame();
    }

    public void Close()
    {
        m_instructionPanel.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    public string getId()
    {
        return id;
    }
}
