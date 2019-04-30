using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinChangeDisplayManager : MonoBehaviour
{
    public static CoinChangeDisplayManager instance;

    [SerializeField] private CoinChangeDisplayUI CoinChangeDisplay;

    private void Awake()
    {
        // initialize singleton
        if(instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) {
            displayCoinChange(new Vector2(100, 100), 2);
        }
    }

    public void displayCoinChange(Vector2 position, int amount)
    {
        CoinChangeDisplayUI display = Instantiate(CoinChangeDisplay);
        display.transform.parent = transform;
        display.transform.position = position;
        display.SetCoinAmount(amount);
    }
}
