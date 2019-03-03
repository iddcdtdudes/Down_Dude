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
    public event Action dudeIsKilledEvent;

    private DudeMode m_dudeMode;
    private bool m_dudeAlive;
    [SerializeField] private float m_jetpackForce;

    [SerializeField] private float m_parachuteForce;
    [SerializeField] private float m_maxParachuteForce;
    [SerializeField] private float m_maxWalkingForce;
    private Vector2 ForceVector;
    private Touch FirstTouch;
    private bool facingRight = false;

    [SerializeField] private Vector2 m_drag;
    [SerializeField] private Vector2 m_maxVelocity;
    [SerializeField] private float m_forceSquareDistance;

    [SerializeField] private Animator m_animator;

    //For restart
    private Rigidbody2D m_defaultDudeRigidbody;
    private Vector3 m_defaultDudePosition;
    private Color m_defaultDudeColor;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_defaultDudeRigidbody = instance.GetComponent<Rigidbody2D>();
        m_defaultDudePosition = instance.transform.position;
        m_defaultDudeColor = instance.GetComponentInChildren<SpriteRenderer>().color;
    }

    #region Update
    private void Update()
    {
        if (m_dudeAlive == true) //When dude is alive
        {
            //Change m_dudeMode accordingly
            ChangeDudeMode();
            /*
            if (Input.touchCount > 0) //Check if screen is touch to set dude mode according to it
            {
                FirstTouch = Input.GetTouch(0); //Receive first touch
                SetDudeMode(DudeMode.PARACHUTE); //Set dude mode to parachute
            }
            else
            {
                SetDudeMode(DudeMode.JETPACK); //Set dude mode to jetpack
            }
            */

            ForceVector = CalculateForceVector(FirstTouch); //Calculate the movement vector to use it in fixedUpdate to move the dude

            // update facing direction
            float velX = GetComponent<Rigidbody2D>().velocity.x;
            if(!facingRight && velX > 0.01f) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = true;
            } else if(facingRight && velX < -0.01f) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                facingRight = false;
            }

            //Update Animation
            ChangeDudeAnimation();
        }
    }

    private void FixedUpdate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        /*
        if (m_dudeMode == DudeMode.PARACHUTE) //Parachute Mode
        {
            rigidbody.AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
        }
        else //Jetpace Mode
        {
            rigidbody.AddForce(Vector2.down * m_jetpackForce);
        }
        */
        switch (m_dudeMode)
        {
            case DudeMode.PARACHUTE:
                rigidbody.AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
                break;
            case DudeMode.JETPACK:
                rigidbody.AddForce(Vector2.down * m_jetpackForce);
                break;
            case DudeMode.WALKING:
                rigidbody.AddForce(ForceVector, ForceMode2D.Force);
                break;
            case DudeMode.IDLE:

                break;
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
        if(collision.collider.CompareTag("Fatal"))
        {
            KillDude();
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

    #region Private Functions
    private Vector2 CalculateForceVector(Touch FirstTouch)
    {
        Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position); //Convert screen touch position to world position
        Vector2 ForceVector; //Vector2 that will be return

        if(Mathf.Abs(TouchPosition.x - instance.transform.position.x) >= m_forceSquareDistance) {
            ForceVector.x = TouchPosition.x - instance.transform.position.x;
        } else {
            ForceVector.x = ((TouchPosition.x - instance.transform.position.x) / m_forceSquareDistance) * ((TouchPosition.x - instance.transform.position.x) / m_forceSquareDistance) * m_forceSquareDistance;
            if(TouchPosition.x - instance.transform.position.x < 0) {
                ForceVector.x *= -1;
            }
        }
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

    private bool IsDudeWalking ()
    {
        if (GetComponent<Rigidbody2D>().CompareTag("Obstacle") == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChangeDudeMode ()
    {
        //Screen is touched
        if (Input.touchCount > 0)
        {
            //Walking
            if (instance.IsDudeWalking() == true)
            {
                FirstTouch = Input.GetTouch(0); //Receive first touch
                m_dudeMode = DudeMode.WALKING;
                Debug.Log("Walking Mode");
            }
            //Parachute
            else
            {
                FirstTouch = Input.GetTouch(0); //Receive first touch
                m_dudeMode = DudeMode.PARACHUTE; //Set dude mode to parachute
                Debug.Log("Parachute Mode");
            }
        }
        //Screen is not touch
        else
        {
            //Idle
            if (instance.IsDudeWalking() == true)
            {
                m_dudeMode = DudeMode.IDLE;
                Debug.Log("Idle Mode");
            }
            //Jetpack
            else
            {
                m_dudeMode = DudeMode.JETPACK; //Set dude mode to jetpack
                Debug.Log("Jetpack Mode");
            }
        }
        /*
        //PARACHUTE
        if (Input.touchCount > 0 && m_dudeMode == DudeMode.JETPACK) //Check if screen is touch to set dude mode according to it
        {
            FirstTouch = Input.GetTouch(0); //Receive first touch
            m_dudeMode = DudeMode.PARACHUTE; //Set dude mode to parachute
            Debug.Log("Parachute Mode");

        }
        //WALKING
        else if (Input.touchCount > 0 && instance.IsDudeWalking() == true)
        {
            FirstTouch = Input.GetTouch(0); //Receive first touch
            m_dudeMode = DudeMode.WALKING;
            Debug.Log("Walking Mode");

        }
        //IDLE
        else if (Input.touchCount == 0 && instance.IsDudeWalking() == true)
        {
            m_dudeMode = DudeMode.IDLE;
            Debug.Log("Idle Mode");

        }
        //JETPACK
        else if (Input.touchCount == 0 && instance.IsDudeWalking() == false)
        {
            m_dudeMode = DudeMode.JETPACK; //Set dude mode to jetpack
            Debug.Log("Jetpack Mode");
        }
        */
    }

    private void ChangeDudeAnimation ()
    {
        switch (m_dudeMode)
        {
            case DudeMode.JETPACK:
                m_animator.ResetTrigger("setParachute");
                m_animator.SetTrigger("setJetpack");
                break;
            case DudeMode.PARACHUTE:
                m_animator.ResetTrigger("setJetpack");
                m_animator.SetTrigger("setParachute");
                break;
            case DudeMode.WALKING:
                m_animator.SetBool("isGrounded", false);
                m_animator.SetBool("isWalking", true);
                break;
            case DudeMode.IDLE:
                m_animator.SetBool("isWalking", false);
                m_animator.SetBool("isGrounded", true);
                break;
        }
    }
    #endregion

    #region Public Functions
    public DudeMode GetDudeMode()
    {
        return m_dudeMode;
    }

    public void SetDudeMode(DudeMode mode)
    {
        switch(mode) {
            case DudeMode.JETPACK:
                m_animator.SetTrigger("setJetpack");
                break;
            case DudeMode.PARACHUTE:
                m_animator.SetTrigger("setParachute");
                break;
        }

        m_dudeMode = mode;
    }

    public bool GetDudeAlive ()
    {
        return m_dudeAlive;
    }
    //Kill dude
    public void KillDude ()
    {
        m_dudeAlive = false;
        instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //instance.gameObject.SetActive(false);

        if(dudeIsKilledEvent != null) {
            dudeIsKilledEvent.Invoke();
        }
    }
    //Restart dude
    public void ResetDude ()
    {
        /*
        m_dudeAlive = true;
        //instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //instance.gameObject.SetActive(true);
        instance.transform.position = Vector3.zero;
        instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        instance.GetComponent<Rigidbody2D>().WakeUp();
        Debug.Log("Dude is reset");
        */
    }
    #endregion
}

public enum DudeMode
{ IDLE, PARACHUTE, JETPACK, WALKING }
