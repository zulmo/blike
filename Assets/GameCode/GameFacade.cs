using UnityEngine;

public static class GameFacade
{
    public static readonly FacadeEvent<PlayerController> PlayerMoved = new FacadeEvent<PlayerController>();
    public static readonly FacadeEvent<PlayerController> BombInputPressed = new FacadeEvent<PlayerController>();
    public static readonly FacadeEvent<Bomb> BombExploded = new FacadeEvent<Bomb>();
    public static readonly FacadeEvent<int> GameOver = new FacadeEvent<int>();
}
