using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderResizer : MonoBehaviour {

	private void Update()
    {
        Vector2 size = GetComponent<SpriteRenderer>().size;
        GetComponent<BoxCollider2D>().size = size;
    }
}
