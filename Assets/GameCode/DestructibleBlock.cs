using UnityEngine;

public class DestructibleBlock : MonoBehaviour, TileContent
{
    [SerializeField]
    private BoxCollider _boxCollider;
    public BoxCollider BoxCollider
    {
        get { return _boxCollider; }
    }

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.DestructibleBlock;
    }
}
