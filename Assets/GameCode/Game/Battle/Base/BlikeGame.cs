using System;
using System.Collections.Generic;
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
    private LastOneStandingView _view;
    
    [SerializeField]
    private Transform[] _spawnPointPositions;

    private List<PlayerController> _players;
    private SpawnPoint[] _spawnPoints;
    private List<Bomb> _activeBombs = new List<Bomb>();

    // TODO: setup dynamically
    private IGameMode _gameMode = new LastOneStandingModel();

    private struct SpawnPoint
    {
        public Vector3 Position;
        public Tile Tile;
    }
    
    public void Awake()
    {
        ApplicationModels.RegisterModel<BattleModel>(new BattleModel());

        _view.Initialize();

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

        var nbSpawns = Math.Min(_spawnPointPositions.Length, playersCount);
        _spawnPoints = new SpawnPoint[nbSpawns];
        for (int i = 0; i < nbSpawns; ++i)
        {
            var tile = GetTile(_spawnPointPositions[i].position);
            _spawnPoints[i].Tile = tile;
            _spawnPoints[i].Position = GetTileCenter(tile);

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
        
        _players = new List<PlayerController>(playerModels.Count);
        for (int i = 0; i < playersCount; ++i)
        {
            var model = playerModels[i];
            var controller = Instantiate(ScriptableObjectsDatabase.PlayerSettings.PlayerPrefab, _groundMesh.transform);
            controller.Initialize(model);

            var playerView = controller.GetComponent<PlayerView>();
            playerView.Initialize(model.Color);

            _players.Add(controller);
        }

        ResetBoard();

        GameFacade.PlayerMoved.Connect(OnPlayerMoved);
        GameFacade.BombExploded.Connect(OnBombExploded);
        GameFacade.BombInputPressed.Connect(OnBombInputPressed);
    }

    private void ResetBoard()
    {
        _spawnPoints.Shuffle();
        for (int i = 0, count = _players.Count; i < count; ++i)
        {
            var controller = _players[i];
            var spawn = _spawnPoints[i];

            var position = spawn.Position;
            position.y += 0.5f * controller.CharacterController.height * controller.transform.localScale.y;
            controller.transform.position = position;
            controller.Coords = spawn.Tile.Coords;
            controller.gameObject.SetActive(true);

            spawn.Tile.Content.Add(controller);
            controller.InputBlocked = false;
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

        GameFacade.RoundStart.Invoke();
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
        _view.Deinitialize();

        GameFacade.PlayerMoved.Disconnect(OnPlayerMoved);
        GameFacade.BombExploded.Disconnect(OnBombExploded);
        GameFacade.BombInputPressed.Disconnect(OnBombInputPressed);

        ApplicationModels.UnregisterModel<BattleModel>();
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
        RemoveBomb(bomb);
        _activeBombs.Remove(bomb);

        // TODO: Range should evolve with the player's bonuses
        var range = 3;
        PropagateExplosion(bomb, -1, 0, range);
        PropagateExplosion(bomb, 1, 0, range);
        PropagateExplosion(bomb, 0, -1, range);
        PropagateExplosion(bomb, 0, 1, range);

        // Check win conditions
        int winnerIndex;
        if (_gameMode.IsRoundOver(_players, out winnerIndex))
        {
            RoundOver(winnerIndex);
        }
    }

    private void RemoveBomb(Bomb bomb)
    {
        var coords = bomb.Coords;
        _tiles[coords.x, coords.y].Content.Remove(bomb);
        Destroy(bomb.gameObject);
    }

    private void RoundOver(int winnerIndex)
    {
        for (int i = 0, count = _players.Count; i < count; ++i)
        {
            var player = _players[i];
            player.InputBlocked = true;
            if(player.gameObject.activeInHierarchy)
            {
                _tiles[player.Coords.x, player.Coords.y].Content.Remove(player);
            }
        }

        for(int i = 0, count = _activeBombs.Count; i < count; ++i)
        {
            RemoveBomb(_activeBombs[i]);
        }
        _activeBombs.Clear();

        var battleModel = ApplicationModels.GetModel<BattleModel>();
        if (winnerIndex >= 0 && ++battleModel.Players[winnerIndex].Score == battleModel.NbRoundsToWin)
        {
            GameFacade.GameOver.Invoke(winnerIndex);
        }
        else
        {
            GameFacade.RoundOver.Invoke(winnerIndex, ResetBoard);
        }
    }

    private void PropagateExplosion(Bomb bomb, int diffX, int diffY, int range)
    {
        var currentDistance = 0;
        var coords = bomb.Coords;
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
                        var tileBomb = tile.Bomb;
                        if (tileBomb)
                        {
                            tileBomb.Explode();
                        }

                        // Handle players hit by the bomb
                        var players = tile.Players;
                        if (players != null)
                        {
                            var battleModels = ApplicationModels.GetModel<BattleModel>().Players;
                            for (int i = 0, count = players.Count; i < count; ++i)
                            {
                                var player = players[i];
                                player.gameObject.SetActive(false);
                                tile.Content.Remove(player);

                                var victim = battleModels.Find(p => p.PlayerModel == player.Owner);
                                _gameMode.OnPlayerHit(victim);
                            }
                        }                        
                    }
                }
            }
        }
        while (canPropagate);
    }

    private void OnBombInputPressed(PlayerController player)
    {
        var position = player.transform.position;
        var tile = GetTile(position);
        if(tile != null && !tile.HasBomb)
        {
            var bomb = Instantiate(ScriptableObjectsDatabase.PlayerSettings.BombPrefab, position, Quaternion.identity, _groundMesh.transform);
            bomb.Initialize(player.Owner, tile.Coords);
            tile.Content.Add(bomb);
            _activeBombs.Add(bomb);
        }
    }
}
