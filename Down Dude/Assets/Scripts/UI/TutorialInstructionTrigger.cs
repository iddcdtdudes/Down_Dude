using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInstructionTrigger : MonoBehaviour
{
    [SerializeField] private string m_instructionId;
    [SerializeField] private bool m_tutorialComplete = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Dude")) {
            ChunkManager.instance.DisplayTutorialChunk(m_instructionId);
            if (m_tutorialComplete) {
                Debug.Log("PASS");
                PlayerDataManager.instance.SetTutorialPlayed();
            }
        }
    }
}
