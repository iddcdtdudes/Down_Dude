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
        transform.position = DudeController.instance.transform.position + new Vector3(0f, m_yOffset, -10f);
    }

}
