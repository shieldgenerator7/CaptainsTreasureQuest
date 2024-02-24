using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapKeeper
{

    private TileMap tileMap;
    public TileMap TileMap => tileMap;

    public int gridWidth;
    public int gridHeight;

    public void switchMap(TileMap map, int gridWidth, int gridHeight)
    {
        this.tileMap = map;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        OnMapChanged?.Invoke(tileMap);
    }
    public Action<TileMap> OnMapChanged;

    public int getXIndex(Vector2 pos)
    {
        return Mathf.RoundToInt(pos.x + gridWidth / 2);
    }

    public int getYIndex(Vector2 pos)
    {
        return Mathf.RoundToInt(pos.y + gridHeight / 2);
    }

    public Vector2 getGridPos(Vector2 worldPos)
    {
        return new Vector2(getXIndex(worldPos), getYIndex(worldPos));
    }

    public Vector2 getWorldPos(Vector2 iv)
    {
        return getWorldPos((int)iv.x, (int)iv.y);
    }
    public Vector2 getWorldPos(Vector2Int iv)
    {
        return getWorldPos(iv.x, iv.y);
    }
    public Vector2 getWorldPos(int ix, int iy)
    {
        Vector2 pos = Vector2.zero;
        pos.x = ix - gridWidth / 2;
        pos.y = iy - gridHeight / 2;
        return pos;
    }

    public LevelTile getTile(Vector2 pos)
    {
        int xIndex = getXIndex(pos);
        int yIndex = getYIndex(pos);
        return tileMap[xIndex, yIndex];
    }
    public Vector2 getPosition(LevelTile lt)
    {
        return getWorldPos(lt.x, lt.y);
    }
}
