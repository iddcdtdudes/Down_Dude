using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChunkWallResizer : MonoBehaviour {

    [SerializeField] private BoxCollider2D m_leftWall;
    [SerializeField] private BoxCollider2D m_rightWall;

    [SerializeField] private GameObject m_chunkTop;
    [SerializeField] private GameObject m_chunkBottom;

    private void Update()
    {
        float chunkHeight = Mathf.Abs(m_chunkTop.transform.position.y - m_chunkBottom.transform.position.y);

        UpdateWallSize(m_leftWall, chunkHeight);
        UpdateWallSize(m_rightWall, chunkHeight);
    }

    // update size of a wall collider
    private void UpdateWallSize(BoxCollider2D wall, float chunkHeight)
    {
        wall.offset = new Vector2(wall.offset.x, -chunkHeight / 2);
        wall.size = new Vector2(wall.size.x, chunkHeight);
    }
}
