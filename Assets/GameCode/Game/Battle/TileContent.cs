public enum TileContentType
{
    Wall,
    DestructibleBlock,
    Player,
    Bomb,
    SpawnArea
}

public interface TileContent
{
    TileContentType GetContentType();
}
