using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    public List<PlayerModel> Players { get; private set; }
    public EGameMode SelectedMode { get; set; }

    public GameModel()
    {
        Players = new List<PlayerModel>();
        SelectedMode = EGameMode.LastOneStanding; // TODO: Set this from a menu
    }

    public void AddPlayer(int joystick, Color color)
    {
        Players.Add(new PlayerModel(joystick, color));
    }
}
