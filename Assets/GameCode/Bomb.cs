using UnityEngine;

public class Bomb : MonoBehaviour, TileContent
{
    [SerializeField]
    private float Timer;

    [SerializeField]
    private int Range;

    public Vector2Int Coords { get; private set; }

    private float Elapsed = 0;

    public void Initialize(Vector2Int coords)
    {
        Coords = coords;
    }

    private void Update ()
    {
        Elapsed += Time.deltaTime;
        if(Elapsed > Timer)
        {
            Explode();
        }
	}

    public void Explode()
    {
        GameFacade.BombExploded.Invoke(this);
    }

    // TileContent interface
    public TileContentType GetContentType()
    {
        return TileContentType.Bomb;
    }
}
