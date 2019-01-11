public class SpawnArea : TileContent
{
    public static readonly SpawnArea Instance = new SpawnArea();

    private SpawnArea()
    {
    }

    public TileContentType GetContentType()
    {
        return TileContentType.SpawnArea;
    }
}