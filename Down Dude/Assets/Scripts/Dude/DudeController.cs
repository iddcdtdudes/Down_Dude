using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DudeController : MonoBehavior
//
// manage Dude's controls
// - jetpack
// - parachute
// - idle
// - walking
// - dying

public class DudeController : MonoBehaviour {
    
    // singleton instance
    public static DudeController instance;

    public event Action reachCheckpointEvent;

    private DudeMode m_dudeMode;
    private bool m_dudeAlive;
    [SerializeField] private float m_jetpackForce;

    [SerializeField] private float m_parachuteForce;
    [SerializeField] private float m_maxParachuteForce;

    private Vector2 ForceVector;
    private Touch FirstTouch;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    #region Update
    private void Update()
    {
        if (m_dudeAlive == true) //When dude is alive
        {
            if (Input.touchCount > 0) //Check if screen is touch to set dude mode according to it
            {
                FirstTouch = Input.GetTouch(0); //Receive first touch
                m_dudeMode = DudeMode.PARACHUTE; //Set dude mode to parachute
            }
            else
            {
                m_dudeMode = DudeMode.JETPACK; //Set dude mode to jetpack
            }

            ForceVector = CalculateForceVector(FirstTouch); //Calculate the movement vector to use it in fixedUpdate to move the dude
        }
        else //When dude is dead
        {
            instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }

    }

    private void FixedUpdate()
    {
        if (m_dudeMode == DudeMode.PARACHUTE) //Parachute Mode
        {
            GetComponent<Rigidbody2D>().AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
        }
        else //Jetpace Mode
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * m_jetpackForce);
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // invoke checkpoint event
        if(collider.CompareTag("Checkpoint"))
        {
            if(reachCheckpointEvent != null) {
                reachCheckpointEvent.Invoke();
            }
            
        }
        else if(collider.CompareTag("Fatal"))
        {
            Debug.Log("Dude is dead");
            instance.SetDudeAlive(false);
        }
    }



    private Vector2 CalculateForceVector(Touch FirstTouch)
    {
        Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position);
        Vector2 ForceVector;

        ForceVector.x = TouchPosition.x - instance.transform.position.x;
        ForceVector.y = 0f;

        if (ForceVector.magnitude > m_maxParachuteForce)
        {
            if (ForceVector.x > 0)
            {
                ForceVector = new Vector2(m_maxParachuteForce, 0f);
            }
            else
            {
                ForceVector = new Vector2(-m_maxParachuteForce, 0f);
            }
        }

        return ForceVector;
    }

    #region Public
    public DudeMode GetDudeMode()
    {
        return m_dudeMode;
    }

    public bool GetDudeAlive ()
    {
        return m_dudeAlive;
    }

    public void SetDudeAlive (bool state)
    {
        m_dudeAlive = state;
    }
    #endregion
}

public enum DudeMode
{ PARACHUTE, JETPACK }
