public enum TileContentType
{
    Wall,
    Player,
    Bomb
}

public interface TileContent
{
    TileContentType GetContentType();
}