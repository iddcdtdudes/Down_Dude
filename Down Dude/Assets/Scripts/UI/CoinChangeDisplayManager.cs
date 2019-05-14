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
            displayCoinChange(new Vector2(100, 100), 2, true);
        }
    }

    public void displayCoinChange(Vector2 position, int amount, bool plus)
    {
        CoinChangeDisplayUI display = Instantiate(CoinChangeDisplay);
        display.transform.SetParent(transform);
        display.transform.position = position;
        if (plus)
        {
            display.PlusCoinAmount(amount);
        }
        else
        {
            display.MinusCoinAmount(amount);
        }
        
    }
}
