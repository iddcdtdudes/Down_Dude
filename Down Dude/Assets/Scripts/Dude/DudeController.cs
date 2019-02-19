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

    [SerializeField] private Vector2 m_drag;
    [SerializeField] private Vector2 m_maxVelocity;
    [SerializeField] private float m_forceSquareDistance;

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
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (m_dudeMode == DudeMode.PARACHUTE) //Parachute Mode
        {
            rigidbody.AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
        }
        else //Jetpace Mode
        {
            rigidbody.AddForce(Vector2.down * m_jetpackForce);
        }

        // apply drag and limit velocity
        Vector2 vel = rigidbody.velocity;
        vel.x = Mathf.Clamp(m_drag.x * vel.x, -m_maxVelocity.x, m_maxVelocity.x);
        vel.y = Mathf.Clamp(m_drag.y * vel.y, -m_maxVelocity.y, m_maxVelocity.y);
        rigidbody.velocity = vel;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // game over upon colliding with fatal hitbox
        if(collision.collider.CompareTag("Fatal")) {
            instance.SetDudeAlive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // invoke checkpoint event
        if(collision.CompareTag("Checkpoint"))
        {
            if(reachCheckpointEvent != null) {
                reachCheckpointEvent.Invoke();
            }
            
        }
    }



    private Vector2 CalculateForceVector(Touch FirstTouch)
    {
        Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position);
        Vector2 ForceVector;

        if(Mathf.Abs(TouchPosition.x - instance.transform.position.x) >= m_forceSquareDistance) {
            ForceVector.x = TouchPosition.x - instance.transform.position.x;
        } else {
            ForceVector.x = ((TouchPosition.x - instance.transform.position.x) / m_forceSquareDistance) * ((TouchPosition.x - instance.transform.position.x) / m_forceSquareDistance) * m_forceSquareDistance;
            if(TouchPosition.x - instance.transform.position.x < 0) {
                ForceVector.x *= -1;
            }
        }
        ForceVector.y = 0f;

        Debug.Log(ForceVector.x);


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
        if(!state) {
            Debug.Log("Game Over");
        }
    }
    #endregion
}

public enum DudeMode
{ PARACHUTE, JETPACK }
