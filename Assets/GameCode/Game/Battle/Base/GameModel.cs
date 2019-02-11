using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    public List<PlayerModel> Players { get; private set; }
    public EGameMode SelectedMode { get; set; }

    //TODO : Should be the level layout
    public bool SpawnBlocks { get; set; }

    public GameModel()
    {
        Players = new List<PlayerModel>();
        SelectedMode = EGameMode.LastOneStanding; // TODO: Set this from a menu
        SpawnBlocks = true;
    }

    public void AddPlayer(int joystick, Color color)
    {
        Players.Add(new PlayerModel(joystick, color));
    }
}
