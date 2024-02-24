using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelTile
{
    public enum Contents
    {
        NONE,
        TRAP,
        TREASURE,
        MAP
    }
    private Contents contents = Contents.NONE;
    public Contents Content
    {
        get => (Walkable)
            ? contents
            : Contents.NONE;
        set
        {
            if (!Locked)
            {
                contents = value;
                if (contents != Contents.NONE)
                {
                    Walkable = true;
                }
            }
        }
    }

    /// <summary>
    /// True if its contents are non-editable
    /// </summary>
    public bool Locked = false;

    /// <summary>
    /// True if it is land, false if it is water
    /// </summary>
    public bool Walkable = false;

    public int x { get => Position.x; }
    public int y { get => Position.y; }
    public readonly Vector2Int Position;

    public Terrain terrain;

    public LevelTile(int x, int y)
    {
        this.Position = new Vector2Int(x, y);
        this.contents = Contents.NONE;
        this.Locked = false;
        this.Walkable = false;
    }

    public bool Available
        => contents == Contents.NONE && !Locked && Walkable;

    public bool Detectable
        => Content == Contents.TRAP
        || Content == Contents.TREASURE;
}
