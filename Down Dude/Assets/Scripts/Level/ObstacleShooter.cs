using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObstacleShooter : StaticObstacle
//
// - shoot out ammo
// - set shoot loop to false to shoot only once

public class ObstacleShooter : StaticObstacle
{
    [SerializeField] private MovingObstacle m_ammo;     // ammo prefab
    [SerializeField] private Vector2 m_ammoVelocity;    // velocity of ammo
    [SerializeField] private bool m_shootLoop;          // shoot in loop / shoot once
    [SerializeField] private float m_shootPeriod;       // time between each shot

    private float m_lastShootTime;
    private bool m_isShooting = true;

    private void Start()
    {
        m_lastShootTime = Time.time;
    }

    private void Update()
    {
        // shoot bullet
        if(m_isShooting && Time.time >= m_lastShootTime + m_shootPeriod) {

            // instantiate bullet
            MovingObstacle bullet = Instantiate(m_ammo);
            bullet.transform.parent = transform;
            bullet.transform.position = transform.position;
            bullet.SetVelocity(m_ammoVelocity);

            // update last shoot time
            m_lastShootTime = Time.time;

            // if shoot once
            if(!m_shootLoop) {
                m_isShooting = false;
            }
        }
    }
}
