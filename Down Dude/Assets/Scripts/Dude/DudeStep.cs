using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeStep : MonoBehaviour
{
    public void OnDudeStep() {
        if(DudeController.instance.GetDudeAlive()) {
            AudioManager.instance.Play("Step");
        }
    }
}
