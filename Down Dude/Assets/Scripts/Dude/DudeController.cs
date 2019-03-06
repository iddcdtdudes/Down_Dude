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
    //Event
    public event Action reachCheckpointEvent;
    public event Action dudeIsKilledEvent;
    //State
    private DudeMode m_dudeMode;
    private bool m_dudeAlive;
    private bool m_dudeIsOnGround;
    //Variable for controlling movements
    [SerializeField] private float m_jetpackForce;
    [SerializeField] private float m_parachuteForce;
    [SerializeField] private float m_walkingForce;
    [SerializeField] private float m_maxParachuteForce;
    //Input
    private Vector2 ForceVector;
    private Touch FirstTouch;
    private bool facingRight = false;
    //Direction
    [SerializeField] private Vector2 m_drag;
    [SerializeField] private Vector2 m_maxVelocity;
    [SerializeField] private float m_forceSquareDistance;
    //Animation
    [SerializeField] private Animator m_animator;

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
        m_dudeIsOnGround = false;
    }

    #region Update
    private void Update()
    {
        if (m_dudeAlive == true) //When dude is alive
        {
            ChangeDudeMode();
            /*
            if (Input.touchCount > 0) //Check if screen is touch to set dude mode according to it
            {
                FirstTouch = Input.GetTouch(0); //Receive first touch
                if(m_dudeMode != DudeMode.PARACHUTE) {
                    SetDudeMode(DudeMode.PARACHUTE); //Set dude mode to parachute
                }
            }
            else
            {
                if(m_dudeMode != DudeMode.JETPACK) {
                    SetDudeMode(DudeMode.JETPACK); //Set dude mode to jetpack
                }
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
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(Application.targetFrameRate);

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        switch (m_dudeMode)
        {
            case DudeMode.JETPACK:
                rigidbody.AddForce(Vector2.down * m_jetpackForce);
                break;
            case DudeMode.PARACHUTE:
                rigidbody.AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
                break;
            case DudeMode.WALKING:
                rigidbody.AddForce(m_walkingForce * ForceVector, ForceMode2D.Force);
                break;
        }

        /*
        if (m_dudeMode == DudeMode.PARACHUTE) //Parachute Mode
        {
            rigidbody.AddForce(m_parachuteForce * ForceVector, ForceMode2D.Force);
        }
        else //Jetpack Mode
        {
            rigidbody.AddForce(Vector2.down * m_jetpackForce);
        }
        */

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

    private void ChangeDudeMode ()
    {
        if (Input.touchCount > 0) //Check if screen is touch to set dude mode according to it
        {
            FirstTouch = Input.GetTouch(0); //Receive first touch
            if (m_dudeIsOnGround)
            {
                if (m_dudeMode != DudeMode.WALKING)
                {
                    SetDudeMode(DudeMode.WALKING);
                    Debug.Log("Walking");
                }
            }
            else 
            {
                if (m_dudeMode != DudeMode.PARACHUTE)
                {
                    SetDudeMode(DudeMode.PARACHUTE); //Set dude mode to parachute
                    Debug.Log("Parachute");
                }
            }
        }
        else
        {
            if (m_dudeIsOnGround)
            {
                if (m_dudeMode != DudeMode.IDLE)
                {
                    SetDudeMode(DudeMode.IDLE);
                    Debug.Log("Idle");
                }
            }
            else
            {
                if (m_dudeMode != DudeMode.JETPACK)
                {
                    SetDudeMode(DudeMode.JETPACK); //Set dude mode to jetpack
                    Debug.Log("Jetpack");
                }
            }
            
        }
    }

    #region Public
    public DudeMode GetDudeMode()
    {
        return m_dudeMode;
    }

    public void SetDudeMode(DudeMode mode)
    {
        switch(mode) {
            case DudeMode.JETPACK:
                m_animator.SetBool("isParachuting", false);
                m_animator.SetBool("isGrounded", false);
                break;
            case DudeMode.PARACHUTE:
                m_animator.SetBool("isParachuting", true);
                break;
            case DudeMode.IDLE:
                m_animator.SetBool("isGrounded", true);
                m_animator.SetBool("isWalking", false);
                break;
            case DudeMode.WALKING:
                m_animator.SetBool("isWalking", true);
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
        instance.GetComponent<Rigidbody2D>().isKinematic = true;

        if(dudeIsKilledEvent != null) {
            dudeIsKilledEvent.Invoke();
        }
    }
    //Restart dude
    public void ResetDude ()
    {
        m_dudeAlive = true;
        instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        instance.GetComponent<Rigidbody2D>().isKinematic = false;
        instance.GetComponent<Rigidbody2D>().WakeUp();
    }

    public void SetDudeWalking (bool i)
    {
        m_dudeIsOnGround = i;
    }
    #endregion
}

public enum DudeMode
{ PARACHUTE, JETPACK , WALKING, IDLE}
