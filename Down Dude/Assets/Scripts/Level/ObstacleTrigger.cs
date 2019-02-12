using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObstacleTrigger : MonoBehaviour
//
// activates listed obstacles when on triggered by Dude

[RequireComponent(typeof(Collider2D))]
public class ObstacleTrigger : MonoBehaviour {

    [SerializeField] private Obstacle[] m_obstacles;

    private bool isTriggered = false;

    private void Start()
    {
        // inactivate all obstacles
        foreach(Obstacle obstacle in m_obstacles) {
            obstacle.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // trigger by Dude
        if(!isTriggered && collider.CompareTag("Dude")) {
            isTriggered = true;

            // activate obstacles
            foreach(Obstacle obstacle in m_obstacles) {
                StartCoroutine(DelayedActivateObstable(obstacle));
            }
        }
    }

    // activate obstacle after delay seconds specified in the Obstacle component
    private IEnumerator DelayedActivateObstable(Obstacle obstacle)
    {
        yield return new WaitForSeconds(obstacle.GetSpawnDelay());
        obstacle.gameObject.SetActive(true);
    }
}
