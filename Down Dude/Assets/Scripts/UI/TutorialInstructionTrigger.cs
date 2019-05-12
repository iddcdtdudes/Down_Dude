using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInstructionTrigger : MonoBehaviour
{
    [SerializeField] private string instructionId;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Dude")) {
            ChunkManager.instance.DisplayTutorialChunk(instructionId);
        }
    }
}
