using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Dude")) {
            OnCollectCoin();
        }
    }

    private void OnCollectCoin()
    {
        // add coin
        PlayerDataManager.instance.AddCoins(1);

        // Add session coin
        GameManager.instance.AddSessionCoin();

        // display coin change
        CoinChangeDisplayManager.instance.displayCoinChange(Camera.main.WorldToScreenPoint(transform.position), 1);

        // sound
        AudioManager.instance.Play("Collect Coin");

        // remove coin
        Destroy(gameObject);
    }
}
