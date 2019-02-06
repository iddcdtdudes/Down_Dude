using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeController : MonoBehaviour {

    private void Update()
    {
        if(Input.touchCount > 0) {

            Touch touch = Input.GetTouch(0);

            Vector2 position = Camera.main.ScreenToWorldPoint(touch.position);

            transform.position = position + new Vector2(0f, 0.5f);
        }

    }
}
