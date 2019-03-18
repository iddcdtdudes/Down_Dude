using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderResizer : MonoBehaviour {

    [SerializeField] private List<ColliderHolder> colliders;

	private void Update()
    {
        Vector2 size = GetComponent<SpriteRenderer>().size;
        
        foreach(ColliderHolder c in colliders) {
            if(c.collider != null) {

                Vector2 originalSize = c.collider.size;

                if(!c.isChild) {
                    c.collider.offset = c.positionOffset;
                    c.collider.size = new Vector2(size.x + c.sizeOffset.x, size.y + c.sizeOffset.y);
                    if(c.fixX) {
                        c.collider.size = new Vector2(originalSize.x, c.collider.size.y);
                    }
                    if(c.fixY) {
                        c.collider.size = new Vector2(c.collider.size.x, originalSize.y);
                    }
                } else {
                    c.collider.transform.position = transform.position + (Vector3)c.positionOffset;
                    c.collider.GetComponent<SpriteRenderer>().size = new Vector2(size.x + c.sizeOffset.x, size.y + c.sizeOffset.y);
                    c.collider.size = new Vector2(size.x + c.sizeOffset.x, size.y + c.sizeOffset.y);
                    if (c.fixX) {
                        c.collider.GetComponent<SpriteRenderer>().size = new Vector2(originalSize.x, c.collider.size.y);
                        c.collider.size = new Vector2(originalSize.x, c.collider.size.y);
                    }
                    if (c.fixY) {
                        c.collider.GetComponent<SpriteRenderer>().size = new Vector2(c.collider.size.x, originalSize.y);
                        c.collider.size = new Vector2(c.collider.size.x, originalSize.y);
                    }
                }
            }
        }
    }
}

[System.Serializable]
class ColliderHolder
{
    public bool isChild;
    public BoxCollider2D collider;
    public Vector2 positionOffset;
    public bool fixX;
    public bool fixY;
    public Vector2 sizeOffset;
}