using System.Collections.Generic;
using UnityEngine;

public class GameModel
{
    public List<PlayerModel> Players { get; private set; }

    public GameModel()
    {
        Players = new List<PlayerModel>();
    }

    public void AddPlayer(int joystick, Color color)
    {
        Players.Add(new PlayerModel(joystick, color));
    }
}
