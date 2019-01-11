using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int Coords { get; private set; }

    public List<TileContent> Content { get; private set; }
    
    public bool IsEmpty(int ignoreFlags = 0)
    {
        return Content.TrueForAll(item => IsContentEmpty(item, ignoreFlags));
    }

    private bool IsContentEmpty(TileContent item, int ignoreFlags)
    {
        var contentFlag = 1 << (int)item.GetContentType();
        return (contentFlag & ignoreFlags) != 0;
    }

    public bool IsWall
    {
        get { return HasContentType(TileContentType.Wall); }
    }

    public bool IsDestructibleBlock
    {
        get { return HasContentType(TileContentType.DestructibleBlock); }
    }

    public DestructibleBlock DestructibleBlock
    {
        get { return (DestructibleBlock)GetContent(TileContentType.DestructibleBlock); }
    }

    public bool HasBomb
    {
        get { return HasContentType(TileContentType.Bomb); }
    }

    public Bomb Bomb
    {
        get { return (Bomb)GetContent(TileContentType.Bomb); }
    }

    public List<PlayerController> Players
    {
        get
        {
            List<PlayerController> returnedPlayers = null;
            for (int i = 0, count = Content.Count; i < count; ++i)
            {
                var content = Content[i];
                if (content.GetContentType() == TileContentType.Player)
                {
                    if(returnedPlayers == null)
                    {
                        returnedPlayers = new List<PlayerController>();
                    }

                    returnedPlayers.Add((PlayerController)content);
                }
            }
            return returnedPlayers;
        }
    }

    public Tile(int x, int y)
    {
        Coords = new Vector2Int(x, y);
        Content = new List<TileContent>();
    }

    private TileContent GetContent(TileContentType contentType)
    {
        return Content.Find(x => x.GetContentType() == contentType);
    }

    private bool HasContentType(TileContentType contentType)
    {
        return GetContent(contentType) != null;
    }
}