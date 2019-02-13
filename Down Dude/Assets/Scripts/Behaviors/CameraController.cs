using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraController : MonoBehavior
//
// controls camera's tracking of Dude

public class CameraController : MonoBehaviour {

    // singleton instance
    public static CameraController instance;

    [SerializeField] private float m_yOffset;


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
        if (DudeController.instance.m_dudeJetpackMode == true && DudeController.instance.m_dudeParachuteMode == false)
        {
            transform.position = DudeController.instance.transform.position + new Vector3(0f, m_yOffset, -10f);
        }
        else
        {
            transform.position = DudeController.instance.transform.position + new Vector3(0f, 0f, -10f);
        }
    }



}
