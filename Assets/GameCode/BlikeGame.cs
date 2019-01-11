using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlikeGame : MonoBehaviour
{
    public static int MAX_PLAYERS = 8;

    private static int NB_COLUMNS = 11;
    private static int NB_ROWS = 11;

    private Tile[,] _tiles = new Tile[NB_COLUMNS, NB_ROWS];

    [SerializeField]
    private GameObject _terrain;

    [SerializeField]
    private MeshRenderer _groundMesh;

    [SerializeField]
    private DestructibleBlock _destructibleBlockPrefab;

    [SerializeField]
    private Bomb _bombPrefab;

    [SerializeField]
    private PlayerController _playerPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    private List<PlayerController> _players;

    private struct SpawnPoint
    {
        public Vector3 Position;
        public Tile Tile;
    }
    
    public void Awake()
    {
        for (int i = 0; i < NB_COLUMNS; ++i)
        {
            for (int j = 0; j < NB_ROWS; ++j)
            {
                _tiles[i, j] = new Tile(i, j);
            }
        }

        var playerModels = ApplicationModels.GetModel<GameModel>().Players;
        int playersCount = playerModels.Count;

        UpdateTilesWithContent<Wall>(_terrain, null);

        var nbSpawns = Math.Min(_spawnPoints.Length, playersCount);
        var spawnPoints = new SpawnPoint[nbSpawns];
        for (int i = 0; i < nbSpawns; ++i)
        {
            var tile = GetTile(_spawnPoints[i].position);
            spawnPoints[i].Tile = tile;
            spawnPoints[i].Position = GetTileCenter(tile);

            // Flag all nearby tiles as SpawnArea to avoid spawning blocks on them
            var coords = tile.Coords;
            for (int offsetX = -1; offsetX <= 1; ++offsetX)
            {
                for (int offsetY = -1; offsetY <= 1; ++offsetY)
                {
                    var x = coords.x + offsetX;
                    var y = coords.y + offsetY;
                    if(x >= 0 && x < NB_COLUMNS && y >= 0 && y < NB_ROWS)
                    {
                        var areaTile = _tiles[x, y];
                        if (!areaTile.IsWall)
                        {
                            areaTile.Content.Add(SpawnArea.Instance);
                        }
                    }
                }
            }
        }
        spawnPoints.Shuffle();
        
        _players = new List<PlayerController>(playerModels.Count);
        for (int i = 0; i < playersCount; ++i)
        {
            var model = playerModels[i];

            var controller = Instantiate(_playerPrefab, _groundMesh.transform);
            var spawn = spawnPoints[i];
            var position = spawn.Position;
            position.y += 0.5f*controller.CharacterController.height*controller.transform.localScale.y;
            controller.transform.position = position;
            controller.JoystickNumber = model.JoystickNumber;
            controller.Coords = spawn.Tile.Coords;

            var playerView = controller.GetComponent<PlayerView>();
            playerView.Initialize(model.Color);

            _players.Add(controller);
            spawn.Tile.Content.Add(controller);
        }
        
        for (int i = 0; i < NB_COLUMNS; ++i)
        {
            for (int j = 0; j < NB_ROWS; ++j)
            {
                var tile = _tiles[i, j];
                if (tile.IsEmpty()) // No ignore flags => tile must be completely empty to receive a block (spawn areas stay empty)
                {
                    var block = Instantiate(_destructibleBlockPrefab, _groundMesh.transform);
                    var position = GetTileCenter(tile);
                    position.y += 0.5f * block.BoxCollider.size.y * block.transform.localScale.y;
                    block.transform.position = position;
                    tile.Content.Add(block);
                }
            }
        }

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

    private Vector3 GetTileCenter(Tile tile)
    {
        var bounds = _groundMesh.bounds;
        var tileSizeX = bounds.size.x / NB_COLUMNS;
        var tileSizeZ = bounds.size.z / NB_ROWS;
        var position = bounds.min;
        position.x += (tile.Coords.x + 0.5f) * tileSizeX;
        position.z += (tile.Coords.y + 0.5f) * tileSizeZ;
        return position;
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

        // Check remaining players
        var activePlayers = _players.FindAll(controller => controller.gameObject.activeInHierarchy);
        var nbActivePlayers = activePlayers.Count;
        if (nbActivePlayers <= 1)
        {
            if (nbActivePlayers == 1)
            {
                Debug.Log(string.Format("Player {0} wins!", _players.FindIndex(o => o == activePlayers[0]) + 1));
            }
            else if (nbActivePlayers == 0)
            {
                Debug.Log("Draw");
            }

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void PropagateExplosion(Vector2Int initialCoords, int diffX, int diffY, int range)
    {
        var currentDistance = 0;
        var coords = initialCoords;
        bool canPropagate = true;

        // Tiles containing only Spawn Areas are considered empty when it comes to explosions
        var contentIgnoreFlags = 1 << (int)TileContentType.SpawnArea;

        do
        {
            coords.x += diffX;
            coords.y += diffY;
            ++currentDistance;
            canPropagate = currentDistance <= range && coords.x >= 0 && coords.x < NB_COLUMNS && coords.y >= 0 && coords.y < NB_ROWS;
            if(canPropagate)
            {
                var tile = _tiles[coords.x, coords.y];
                if(!tile.IsEmpty(contentIgnoreFlags))
                {
                    if(tile.IsWall)
                    {
                        canPropagate = false;
                    }
                    else if (tile.IsDestructibleBlock)
                    {
                        var block = tile.DestructibleBlock;
                        tile.Content.Remove(block);
                        Destroy(block.gameObject);
                        // TODO: Spawn bonus

                        canPropagate = false; // TODO: Depends on the player's bonus (spiked bomb?)
                    }
                    else
                    {
                        // TODO: determine if the explosion propagates when it hits a player or a bomb.
                        // For the moment, let's say yes
                        var bomb = tile.Bomb;
                        if (bomb)
                        {
                            bomb.Explode();
                        }

                        // Kill players hit by the bomb
                        var players = tile.Players;
                        if (players != null)
                        {
                            for (int i = 0, count = players.Count; i < count; ++i)
                            {
                                var player = players[i];
                                player.gameObject.SetActive(false);
                                tile.Content.Remove(player);
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
