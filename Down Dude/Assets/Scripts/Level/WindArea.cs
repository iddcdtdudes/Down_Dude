using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WindArea : StaticObstacle
//
// accelerates dude toward a certain direction when in the area
// if dude is in parachute mode, dude accelerates more

public class WindArea : StaticObstacle
{
    [SerializeField] private Vector2 wind;
    [SerializeField] private float jetpackForce;
    [SerializeField] private float parachuteForce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dude")) {
            if(DudeController.instance.GetDudeMode() == DudeMode.PARACHUTE) {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(parachuteForce * wind, ForceMode2D.Force);
            } else if(DudeController.instance.GetDudeMode() == DudeMode.JETPACK) {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(jetpackForce * wind, ForceMode2D.Force);
            }
        }
    }
}
