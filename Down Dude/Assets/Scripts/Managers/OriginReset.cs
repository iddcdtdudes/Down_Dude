using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginReset : MonoBehaviour {

    public static OriginReset instance;

    private List<GameObject> m_gameObjectToReset;

    [SerializeField] private float m_positionLimit;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_gameObjectToReset = new List<GameObject>();
    }

    // Use this for initialization
    void Start () {
        //Add Player
        m_gameObjectToReset.Add(DudeController.instance.gameObject);
        //Add Camera
        m_gameObjectToReset.Add(CameraController.instance.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(DudeController.instance.gameObject.transform.position.y) > m_positionLimit)
        {
            //Reset
            ResetPosition();
        }
	}

    private void ResetPosition ()
    {
        foreach (GameObject ObjectToReset in m_gameObjectToReset)
        {
            ObjectToReset.transform.position = Vector3.zero;
        }
        
    }

    public void AddObjectToReset (GameObject ObjectToReset)
    {
        m_gameObjectToReset.Add(ObjectToReset);
    }
}