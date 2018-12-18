public enum TileContentType
{
    Wall,
    DestructibleBlock,
    Player,
    Bomb
}

public interface TileContent
{
    TileContentType GetContentType();
}