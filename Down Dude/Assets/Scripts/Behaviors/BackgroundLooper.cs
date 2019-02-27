using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BackgroundLooper : MonoBehavior
//
// - assign background sprite containers to m_nextBackground and m_otherBackground
// - set background height to the sprite's height
// - the background sprites will loop as dude falls down

public class BackgroundLooper : MonoBehaviour {

    // 2 background sprites
    [SerializeField] private GameObject m_nextBackground;
    [SerializeField] private GameObject m_otherBackground;

    [SerializeField] private float m_backgroundHeight;  // height of background sprite


    private void Start()
    {
        // initialize sprite positions
        InitializePosition();
    }

    private void Update()
    {
        // move other background down
        if(DudeController.instance.transform.position.y < m_nextBackground.transform.position.y) {
            m_otherBackground.transform.position = new Vector3(0.0f, m_nextBackground.transform.position.y - m_backgroundHeight, 0.0f);

            GameObject temp = m_nextBackground;
            m_nextBackground = m_otherBackground;
            m_otherBackground = temp;
        }
    }

    public void InitializePosition()
    {
        m_otherBackground.transform.position = new Vector3(0.0f, DudeController.instance.transform.position.y, 0.0f);
        m_nextBackground.transform.position = new Vector3(0.0f, DudeController.instance.transform.position.y - m_backgroundHeight, 0.0f);
    }
}
