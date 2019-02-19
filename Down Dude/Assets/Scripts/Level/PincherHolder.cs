using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PincherHolder : MonoBehaviour {

    [SerializeField] private PinchingObstacle m_pincher1;   // reference to pinching obstacle 1
    [SerializeField] private PinchingObstacle m_pincher2;   // reference to pinching obstacle 2

    private int m_collisionCount = 0;                       // pinching obstacles colliding with Dude

    private void Start()
    {
        // subscribe to pincher events
        m_pincher1.DudeCollisionEnterEvent += OnDudeCollisionEnter;
        m_pincher1.DudeCollisionExitEvent += OnDudeCollisionExit;
        m_pincher2.DudeCollisionEnterEvent += OnDudeCollisionEnter;
        m_pincher2.DudeCollisionExitEvent += OnDudeCollisionExit;
    }

    private void Update()
    {
        // if 2 pinchers touch Dude, game over
        if(m_collisionCount >= 2 && !m_pincher1.GetIsStopped() && !m_pincher2.GetIsStopped()) {
            DudeController.instance.KillDude();
        }
    }

    private void OnDudeCollisionEnter()
    {
        m_collisionCount++;
    }

    private void OnDudeCollisionExit()
    {
        m_collisionCount--;
    }
}
