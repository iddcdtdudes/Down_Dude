using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameManager : MonoBehabiour
//
// keeps track and control states of the game
// - timer
// - scoring
// - checkpoints
// - chunk manager's activities

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake()
    {
        // initialize singleton instance
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }
}
