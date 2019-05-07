using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSequence : MonoBehaviour
{
    public static StartSequence instance;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void GameStart()
    {
        GetComponent<Animator>().SetTrigger("jump");
    }

    public void OnDudeStartWalking()
    {
        DudeController.instance.GetComponent<Animator>().SetTrigger("fadeIn");
        DudeController.instance.SetDudeMode(DudeMode.WALKING);
    }

    public void OnDudeStopWalking()
    {
        DudeController.instance.SetDudeMode(DudeMode.IDLE);
    }

    public void OnDudeJump()
    {
        GameManager.instance.OnDudeJump();
    }
}
