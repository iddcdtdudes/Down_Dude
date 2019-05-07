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

    [SerializeField] private float m_parallax;          // parallax value (0.0 = static, 1.0 = dude)

    [SerializeField] private float m_backgroundHeight;  // height of background sprite
    
    [SerializeField] private bool m_looping = true;     // looping background

    private float m_lastCamY;

    private bool m_firstActivated;
    private bool m_rising = true;

    private void Start()
    {
        // initialize sprite positions
        InitializePosition(!m_looping);

        // initialize cam y
        m_lastCamY = CameraController.instance.transform.position.y;

        m_firstActivated = true;
    }

    private void FixedUpdate()
    {
        if(m_parallax > 0.0f) {

            // update camera velocity
            float camY = CameraController.instance.transform.position.y;
            float camYVel;
            if(!m_firstActivated) {
                camYVel = camY - m_lastCamY;
                m_lastCamY = camY;
            } else {
                m_lastCamY = camY;
                camYVel = 0f;
                m_firstActivated = false;
            }

            // parallax
            //Debug.Log("before " + m_otherBackground.transform.position.y + " " + camYVel);
            m_otherBackground.transform.position = new Vector3(0.0f, m_otherBackground.transform.position.y + Time.deltaTime * (1 - m_parallax) * 30 * camYVel, 0.0f);
            m_nextBackground.transform.position = new Vector3(0.0f, m_nextBackground.transform.position.y +  Time.deltaTime * (1 - m_parallax) * 30 * camYVel, 0.0f);
            //Debug.Log("after  " + m_otherBackground.transform.position.y);
        }
        
    }

    private void Update()
    {
        // move other background down
        if (m_looping && DudeController.instance.transform.position.y < m_nextBackground.transform.position.y) {
            m_otherBackground.transform.position = new Vector3(0.0f, m_nextBackground.transform.position.y - m_backgroundHeight, 0.0f);

            GameObject temp = m_nextBackground;
            m_nextBackground = m_otherBackground;
            m_otherBackground = temp;
        }

        // rising
        if(m_rising) {
            m_otherBackground.transform.position = new Vector3(0.0f, m_otherBackground.transform.position.y + Time.unscaledDeltaTime * m_parallax * -2.0f, 0.0f);
            m_nextBackground.transform.position =  new Vector3(0.0f, m_nextBackground.transform.position.y +  Time.unscaledDeltaTime * m_parallax * -2.0f, 0.0f);

            if(m_looping && Camera.main.transform.position.y > m_otherBackground.transform.position.y) {
                m_nextBackground.transform.position = new Vector3(0.0f, m_otherBackground.transform.position.y + m_backgroundHeight, 0.0f);

                GameObject temp = m_nextBackground;
                m_nextBackground = m_otherBackground;
                m_otherBackground = temp;
            }
        }

        if(m_parallax == 0.0f) {
            m_otherBackground.transform.position = new Vector3(0.0f, CameraController.instance.transform.position.y, 0.0f);
            m_nextBackground.transform.position = new Vector3(0.0f, CameraController.instance.transform.position.y, 0.0f);
        }
    }

    public void InitializePosition(bool overlap)
    {
        m_otherBackground.transform.position = new Vector3(0.0f, CameraController.instance.transform.position.y, 0.0f);
        if(!overlap) {
            m_nextBackground.transform.position = new Vector3(0.0f, CameraController.instance.transform.position.y - m_backgroundHeight, 0.0f);
        } else {
            m_nextBackground.transform.position = new Vector3(0.0f, CameraController.instance.transform.position.y, 0.0f);
        }
    }

    public void SetRising(bool value)
    {
        m_rising = value;
    }

    public float GetBackgroundY()
    {
        return m_otherBackground.transform.position.y;
    }

    public void SetFirstActivated(bool value)
    {
        m_firstActivated = value;
    }
}
