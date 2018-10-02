using System;
using UnityEngine;

public class BlikeGame : MonoBehaviour 
{
    private static int NB_COLUMNS = 11;
    private static int NB_ROWS = 11;

    private Tile[,] _tiles = new Tile[NB_COLUMNS, NB_ROWS];

    [SerializeField]
    private GameObject _terrain;

    [SerializeField]
    private MeshRenderer _groundMesh;

    [SerializeField]
    private Bomb _bombPrefab;

    public void Awake()
    {
        for (int i = 0; i < NB_COLUMNS; ++i)
        {
            for (int j = 0; j < NB_ROWS; ++j)
            {
                _tiles[i, j] = new Tile(i, j);
            }
        }

        UpdateTilesWithContent<Wall>(_terrain, null);
        UpdateTilesWithContent<PlayerController>(gameObject, (controller, tile) => controller.Coords = tile.Coords);

        GameFacade.PlayerMoved.Connect(OnPlayerMoved);
        GameFacade.BombExploded.Connect(OnBombExploded);
        GameFacade.BombInputPressed.Connect(OnBombInputPressed);
    }

    private void OnPlayerMoved(PlayerController controller)
    {
        var coords = controller.Coords;
        var previousTile = _tiles[coords.x, coords.y];
        var newTile = GetTile(controller.transform.position);
        if(newTile != previousTile)
        {
            controller.Coords = newTile.Coords;
            previousTile.Content.Remove(controller);
            newTile.Content.Add(controller);
        }
    }

    public void OnDestroy()
    {
        GameFacade.PlayerMoved.Disconnect(OnPlayerMoved);
        GameFacade.BombExploded.Disconnect(OnBombExploded);
        GameFacade.BombInputPressed.Disconnect(OnBombInputPressed);
    }

    private void UpdateTilesWithContent<TContent>(GameObject parent, Action<TContent, Tile> initializeAction) where TContent : MonoBehaviour, TileContent
    {
        var components = parent.GetComponentsInChildren<TContent>();
        for (int i = 0, count = components.Length; i < count; ++i)
        {
            var component = components[i];
            var tile = GetTile(component.transform.position);
            if (tile != null)
            {
                initializeAction?.Invoke(component, tile);
                tile.Content.Add(component);
            }
        }
    }

    public Tile GetTile(Vector3 position)
    {
        var bounds = _groundMesh.bounds;
        var tileSizeX = bounds.size.x / NB_COLUMNS;
        var tileSizeZ = bounds.size.z / NB_ROWS;
        var posDiff = position - bounds.min;
        var tileX = (int)Math.Min(NB_COLUMNS - 1, Math.Max(0, posDiff.x / tileSizeX));
        var tileY = (int)Math.Min(NB_ROWS - 1, Math.Max(0, posDiff.z / tileSizeZ));
        return _tiles[tileX, tileY];
    }

    private void OnBombExploded(Bomb bomb)
    {
        var coords = bomb.Coords;
        var tile = _tiles[coords.x, coords.y];
        tile.Content.Remove(bomb);
        Destroy(bomb.gameObject);

        // TODO: Range should evolve with the player's bonuses
        var range = 3;
        PropagateExplosion(coords, -1, 0, range);
        PropagateExplosion(coords, 1, 0, range);
        PropagateExplosion(coords, 0, -1, range);
        PropagateExplosion(coords, 0, 1, range);
    }

    private void PropagateExplosion(Vector2Int initialCoords, int diffX, int diffY, int range)
    {
        var currentDistance = 0;
        var coords = initialCoords;
        bool canPropagate = true;
        do
        {
            coords.x += diffX;
            coords.y += diffY;
            ++currentDistance;
            canPropagate = currentDistance <= range && coords.x >= 0 && coords.x < NB_COLUMNS && coords.y >= 0 && coords.y < NB_ROWS;
            if(canPropagate)
            {
                var tile = _tiles[coords.x, coords.y];
                if(!tile.IsEmpty)
                {
                    if(tile.IsWall)
                    {
                        canPropagate = false;
                    }
                    else
                    {
                        // TODO: determine if the explosion propagates when it hits a player of a bomb.
                        // For the moment, let's say yes
                        var bomb = tile.Bomb;
                        if (bomb)
                        {
                            bomb.Explode();
                        }

                        var players = tile.Players;
                        if (players != null)
                        {
                            for (int i = 0, count = players.Count; i < count; ++i)
                            {
                                // TODO: Damage player
                                Debug.Log("Player hit");
                            }
                        }
                    }
                }
            }
        }
        while (canPropagate);
    }

    private void OnBombInputPressed(Vector3 position)
    {
        var tile = GetTile(position);
        if(tile != null && !tile.HasBomb)
        {
            var bomb = Instantiate(_bombPrefab, position, Quaternion.identity, _groundMesh.transform);
            bomb.Initialize(tile.Coords);
            tile.Content.Add(bomb);
        }
    }
}
