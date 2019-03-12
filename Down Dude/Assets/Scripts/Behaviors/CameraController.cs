using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraController : MonoBehavior
//
// controls camera's tracking of Dude

public class CameraController : MonoBehaviour {

    // singleton instance
    public static CameraController instance;

    [SerializeField] private float m_yOffsetTargetJetpack;
    [SerializeField] private float m_yOffsetTargetParachute;
    [SerializeField] private float m_yOffsetSmooth;

    private float m_yOffset;


    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (DudeController.instance.GetDudeMode() == DudeMode.JETPACK)
        {
            m_yOffset = Mathf.Lerp(m_yOffset, m_yOffsetTargetJetpack, m_yOffsetSmooth);
        }
        else
        {
            m_yOffset = Mathf.Lerp(m_yOffset, m_yOffsetTargetParachute, m_yOffsetSmooth);
        }

    }

    private void FixedUpdate()
    {
        if (DudeController.instance.GetDudeAlive() == true)
        {
            transform.position = new Vector3(0f, DudeController.instance.transform.position.y + m_yOffset, -10f);
        }
    }

    public float GetOffsetSmooth()
    {
        Debug.Log(m_yOffsetTargetParachute - m_yOffset);
        return m_yOffsetTargetParachute - m_yOffset;
    }
}
