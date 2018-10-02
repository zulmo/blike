using UnityEngine;

public static class GameFacade
{
    public static readonly FacadeEvent<PlayerController> PlayerMoved = new FacadeEvent<PlayerController>();
    public static readonly FacadeEvent<Vector3> BombInputPressed = new FacadeEvent<Vector3>();
    public static readonly FacadeEvent<Bomb> BombExploded = new FacadeEvent<Bomb>();
}
