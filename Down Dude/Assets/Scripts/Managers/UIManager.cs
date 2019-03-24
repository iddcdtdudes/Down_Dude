using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [Header("Game Over UI")]
    public Text m_sessionHS;
    public Text m_allTimeHS;
    public Text m_sessionCP;
    public Text m_allTimeCP;

    // Start is called before the first frame update
    void Awake()
    {
        // initialize singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DudeController.instance.dudeIsKilledEvent += UpdateGameOverUI;
    }

    public void UpdateGameOverUI()
    {
        m_sessionHS.text = GameManager.instance.GetSessionScores().ToString();
        m_sessionCP.text = GameManager.instance.GetSessionCheckpoints().ToString();
        m_allTimeHS.text = PlayerDataManager.instance.GetAllTimeHS().ToString();
        m_allTimeCP.text = PlayerDataManager.instance.GetAllTimeCP().ToString();
    }

}
