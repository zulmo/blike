using UnityEngine;

public class Wall : MonoBehaviour, TileContent
{
    public TileContentType GetContentType()
    {
        return TileContentType.Wall;
    }
}
