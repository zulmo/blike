using System;

public static class GameFacade
{
    public static readonly FacadeEvent<int> TimeRemainingChanged = new FacadeEvent<int>();
    public static readonly FacadeEvent<PlayerBattleModel> SpawnPlayer = new FacadeEvent<PlayerBattleModel>();
    public static readonly FacadeEvent<PlayerController> PlayerMoved = new FacadeEvent<PlayerController>();
    public static readonly FacadeEvent<PlayerController> BombInputPressed = new FacadeEvent<PlayerController>();
    public static readonly FacadeEvent<Bomb> BombExploded = new FacadeEvent<Bomb>();
    public static readonly FacadeEvent<Bomb, PlayerController> PlayerHit = new FacadeEvent<Bomb, PlayerController>();
    public static readonly FacadeEvent RoundStart = new FacadeEvent();
    public static readonly FacadeEvent<int, bool> BattleEnd = new FacadeEvent<int, bool>();
    public static readonly FacadeEvent<int, Action> RoundOver = new FacadeEvent<int, Action>();
    public static readonly FacadeEvent<int> GameOver = new FacadeEvent<int>();
}
