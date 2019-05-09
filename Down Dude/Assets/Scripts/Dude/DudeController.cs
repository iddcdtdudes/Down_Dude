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
    private DudeDirection m_dudeDir;
    private DudeState m_dudeState;
    private bool m_dudeAlive;
    private bool m_dudeIsOnGround;

    //Alternate Control
    [SerializeField]private bool m_dudeControlByButton;
    [SerializeField] private GameObject m_dudeControlUI;
    [SerializeField] private GameObject m_buttonLeft;
    [SerializeField] private GameObject m_buttonCenter;
    [SerializeField] private GameObject m_buttonRight;
    private bool m_buttonIsPressed;

    //Variable for controlling movements
    [SerializeField] private float m_jetpackForce;
    [SerializeField] private float m_parachuteForce;
    [SerializeField] private float m_walkingForce;
    [SerializeField] private float m_maxParachuteForce;

    //Input
    private Vector2 m_forceVector;
    private Touch FirstTouch;
    private bool facingRight = false;

    //Direction
    [SerializeField] private Vector2 m_drag;
    [SerializeField] private Vector2 m_maxVelocity;
    [SerializeField] private float m_touchForceThreshold;

    //Animation
    [SerializeField] private Animator m_animator;

    // new movement variables
    [SerializeField] private float m_jetpackHorizontalDrag;

    //Dude Dead Prefab
    [SerializeField] private GameObject m_dudeCorpse;

    private float m_lastCheckpointTime;

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
        m_lastCheckpointTime = Time.time;
        m_dudeState = DudeState.NONE;

        if (PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Control))
        {
            if (PlayerPrefs.GetInt(PlayerDataManager.instance.m_playerPref_Control) == 1)
            {
                m_dudeControlByButton = true;
                //Debug.Log("Set to Control by Button");
            }
            else
            {
                m_dudeControlByButton = false;
            }
        }

        SetDudeMode(DudeMode.IDLE);
    }

    #region Update
    private void Update()
    {
        //if (m_dudeAlive == true) //When dude is alive
        if (m_dudeState == DudeState.ALIVE)
        {
            if (m_dudeControlByButton)
            {
                //Change Dude Mode
                CheckButtonPressed();
                ChangeDudeModeButton();
                //Change Moving direction Vector
                switch (m_dudeDir)
                {
                    case DudeDirection.LEFT:
                        m_forceVector.x = -1 * m_maxParachuteForce;
                        break;
                    case DudeDirection.RIGHT:
                        m_forceVector.x = 1 * m_maxParachuteForce;
                        break;
                    case DudeDirection.CENTER:
                        m_forceVector.x = 0;
                        break;
                }
            }
            else
            {
                //Change Dude Mode
                ChangeDudeMode();
                
                //Change Moving Direction Vector
                m_forceVector = CalculateForceVector(FirstTouch); //Calculate the movement vector to use it in fixedUpdate to move the dude

                /*
                // move dude
                if(m_dudeMode == DudeMode.PARACHUTE) {
                    float touchX = Camera.main.ScreenToWorldPoint(FirstTouch.position).x;

                    // move amount
                    float moveAmount = ((touchX - transform.position.x >= 0.0f) ? 1.0f : -1.0f) * m_parachuteMaxHorizontalSpeed;

                    // if touch is closer than threshold, scale down move amount
                    if (Mathf.Abs(touchX - transform.position.x) < m_touchForceThreshold) {
                        moveAmount *= Mathf.Abs(touchX - transform.position.x);
                    }

                    // move dude
                    transform.position += Vector3.right * moveAmount;
                }
                */
            }

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
        if (m_dudeState == DudeState.ALIVE)
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            switch (m_dudeMode)
            {
                case DudeMode.JETPACK:
                    rigidbody.AddForce(Vector2.down * m_jetpackForce);
                    break;
                case DudeMode.PARACHUTE:
                    rigidbody.AddForce(m_parachuteForce * m_forceVector, ForceMode2D.Force);
                    break;
                case DudeMode.WALKING:
                    rigidbody.AddForce(m_walkingForce * m_forceVector, ForceMode2D.Force);
                    break;
            }


            // apply drag and limit velocity
            Vector2 vel = rigidbody.velocity;
            vel.x = Mathf.Clamp(m_drag.x * vel.x, -m_maxVelocity.x, m_maxVelocity.x);
            vel.y = Mathf.Clamp(m_drag.y * vel.y, -m_maxVelocity.y, m_maxVelocity.y);
            rigidbody.velocity = vel;

            // horizontal drag
            float drag = 1.0f;
            if (m_dudeMode == DudeMode.JETPACK)
            {
                drag = m_jetpackHorizontalDrag;
            }
            else if (m_dudeMode == DudeMode.PARACHUTE || m_dudeMode == DudeMode.WALKING)
            {
                // apply horizontal drag when touch is near dude
                float dudeToTouch = Camera.main.ScreenToWorldPoint(FirstTouch.position).x - transform.position.x;
                if (!m_dudeControlByButton && Mathf.Abs(dudeToTouch) < m_touchForceThreshold)
                {
                    if (m_touchForceThreshold != 0.0f)
                    {
                        drag = Mathf.Abs(dudeToTouch / m_touchForceThreshold);
                    }
                }
            }

            rigidbody.velocity = new Vector2(drag * rigidbody.velocity.x, rigidbody.velocity.y);
        }
        

    }
    #endregion

    #region Private
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
        // game over upon colliding with fatal hitbox
        if (collision.CompareTag("Fatal")) {
            KillDude();
        }

        // invoke checkpoint event
        if (collision.CompareTag("Checkpoint") && Time.time >= m_lastCheckpointTime + 1f)
        {
            if(reachCheckpointEvent != null) {
                reachCheckpointEvent.Invoke();
            }
            m_lastCheckpointTime = Time.time;
        }
    }

    private Vector2 CalculateForceVector(Touch FirstTouch)
    {
        Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position); //Convert screen touch position to world position
        Vector2 ForceVector; //Vector2 that will be return

        // if touch is more than touch force threshold
        if (Mathf.Abs(TouchPosition.x - instance.transform.position.x) >= m_touchForceThreshold) {
            // apply force
            ForceVector.x = TouchPosition.x - instance.transform.position.x;
        } else {
            ForceVector.x = ((TouchPosition.x - instance.transform.position.x) / m_touchForceThreshold) * ((TouchPosition.x - instance.transform.position.x) / m_touchForceThreshold) * m_touchForceThreshold;
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
                    //Debug.Log("Walking");
                }
            }
            else 
            {
                if (m_dudeMode != DudeMode.PARACHUTE)
                {
                    SetDudeMode(DudeMode.PARACHUTE); //Set dude mode to parachute
                    //Debug.Log("Parachute");
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
                    //Debug.Log("Idle");
                }
            }
            else
            {
                if (m_dudeMode != DudeMode.JETPACK)
                {
                    SetDudeMode(DudeMode.JETPACK); //Set dude mode to jetpack
                    //Debug.Log("Jetpack");
                }
            }
            
        }
    }

    private void ChangeDudeModeButton ()
    {

        if (m_buttonIsPressed)
        {
            if (m_dudeIsOnGround)
            {
                if (m_dudeMode != DudeMode.WALKING)
                {
                    SetDudeMode(DudeMode.WALKING);
                    //Debug.Log("Walking");
                }
            }
            else
            {
                if (m_dudeMode != DudeMode.PARACHUTE)
                {
                    SetDudeMode(DudeMode.PARACHUTE); //Set dude mode to parachute
                    //Debug.Log("Parachute");
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
                    //Debug.Log("Jetpack");
                }
            }
        }
    }

    private void CheckButtonPressed ()
    {
        if (Input.touchCount > 0)
        {
            Touch finger = Input.GetTouch(0);
            Vector2 touchPosition = finger.position;
            
            //If the x position of touch is on the button
            if(finger.phase != TouchPhase.Ended)
            {
                //Left
                if (RectTransformUtility.RectangleContainsScreenPoint(m_buttonLeft.GetComponent<RectTransform>(), touchPosition))
                {
                    //Debug.Log("Left Button");
                    m_buttonIsPressed = true;
                    m_dudeDir = DudeDirection.LEFT;
                }
                //Center
                else if (RectTransformUtility.RectangleContainsScreenPoint(m_buttonCenter.GetComponent<RectTransform>(), touchPosition))
                {
                    //Debug.Log("Center Button");
                    m_buttonIsPressed = true;
                    m_dudeDir = DudeDirection.CENTER;
                }
                //Right
                else if (RectTransformUtility.RectangleContainsScreenPoint(m_buttonRight.GetComponent<RectTransform>(), touchPosition))
                {
                    //Debug.Log("Right Button");
                    m_buttonIsPressed = true;
                    m_dudeDir = DudeDirection.RIGHT;
                }
            }
            else
            {
                m_buttonIsPressed = false;
            }
        }
        
    }

    #endregion

    #region Public
    public DudeMode GetDudeMode()
    {
        return m_dudeMode;
    }

    public DudeState GetDudeState ()
    {
        return m_dudeState;
    }

    public void ChangeControlToButton (bool i)
    {
        m_dudeControlByButton = i;
        if (m_dudeControlByButton)
        {
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Control, 1);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Control, 0);
        }

        PlayerPrefs.Save();
        
    }

    public void ChangeControlToButton (float i)
    {
        if (i >= 0.5)
        {
            m_dudeControlByButton = true;
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Control, 1);
        }
        else
        {
            m_dudeControlByButton = false;
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Control, 0);
        }
        PlayerPrefs.Save();

    }

    public void ShowButtonUI ()
    {
        if (m_dudeControlByButton)
        {
            m_dudeControlUI.SetActive(true);
            //m_dudeControlUI.GetComponent<Animator>().SetBool("ControlUI", true);
        }
        else
        {
            m_dudeControlUI.SetActive(false);
        }
    }

    public void HideButtonUI ()
    {
        if (m_dudeControlByButton)
        {
            m_dudeControlUI.SetActive(false);
            
        }
    }

    public void SetDudeMode(DudeMode mode)
    {
        switch(mode) {
            case DudeMode.JETPACK:
                m_animator.SetBool("isParachuting", false);
                m_animator.SetBool("isGrounded", false);
                AudioManager.instance.Play("Jetpack");
                break;
            case DudeMode.PARACHUTE:
                m_animator.SetBool("isGrounded", false);
                m_animator.SetBool("isParachuting", true);
                AudioManager.instance.Play("Parachute");
                break;
            case DudeMode.IDLE:
                m_animator.SetBool("isGrounded", true);
                m_animator.SetBool("isWalking", false);
                break;
            case DudeMode.WALKING:
                m_animator.SetBool("isGrounded", true);
                m_animator.SetBool("isWalking", true);
                break;

        }

        m_dudeMode = mode;
    }

    public bool GetDudeAlive ()
    {
        switch (m_dudeState)
        {
            case DudeState.ALIVE:
                return true;
            default:
                return false;
        }
        //return m_dudeAlive;
    }

    //Kill dude
    public void KillDude ()
    {
        //if (m_dudeAlive != false)
        if (m_dudeState != DudeState.DEAD)
        {
            AudioManager.instance.Play("DudeDead");
            AudioManager.instance.Play("Menu");
            AudioManager.instance.StopSound("BGM");
            AudioManager.instance.GameSound(false);
            m_dudeState = DudeState.DEAD;
            instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            //instance.GetComponent<Rigidbody2D>().isKinematic = true;
            instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //m_dudeAlive = false;
            
            DudeKilledSequence();

            if (dudeIsKilledEvent != null)
            {
                dudeIsKilledEvent.Invoke();
            }

        }

        
        
        //

        
    }
    //Restart dude
    public void ResetDude ()
    {
        //m_dudeAlive = true;
        m_dudeState = DudeState.NONE;
        AudioManager.instance.GameSound(true);
        instance.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        instance.GetComponent<Rigidbody2D>().isKinematic = false;
        instance.GetComponent<Rigidbody2D>().WakeUp();
    }

    public void SetDudeState (DudeState m)
    {
        m_dudeState = m;
    }

    public void SetDudeWalking (bool i)
    {
        m_dudeIsOnGround = i;
    }
    
    public void DudeKilledSequence ()
    {
        GameObject corpse = Instantiate(m_dudeCorpse, transform.position, transform.rotation);
        corpse.transform.position = transform.position;
        corpse.transform.rotation = transform.rotation;
        corpse.GetComponentInChildren<Animator>().runtimeAnimatorController = SkinManager.instance.GetSkinAnimator(PlayerDataManager.instance.GetUsingSkin());
        Rigidbody2D rigidbody = corpse.GetComponent<Rigidbody2D>();

        rigidbody.velocity = new Vector2(0.0f, 0.0f);

        rigidbody.AddForce(new Vector2(0.0f, 2.5f), ForceMode2D.Impulse);
        rigidbody.AddTorque(500.0f);
        
        Debug.Log("kill dude");

        StartCoroutine(GameOverScreen());
        
    }

    IEnumerator GameOverScreen()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        GameManager.instance.GameOverUI();
        yield return new WaitForSecondsRealtime(1.0f);
        UIManager.instance.StartGameOverUI();
        //gameObject.SetActive(false);
        //corpse.GetComponent<Rigidbody2D>().isKinematic = true;

    }

    public void OnDudeFadeInComplete()
    {
        GetComponent<Animator>().enabled = false;
    }

    #endregion
}

public enum DudeMode
{ PARACHUTE, JETPACK , WALKING, IDLE}

public enum DudeDirection
{ LEFT, CENTER, RIGHT }

public enum DudeState
{ NONE, ALIVE, DEAD }
