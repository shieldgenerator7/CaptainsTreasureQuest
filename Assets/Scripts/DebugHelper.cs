﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    public LevelTile.Contents selectContent;
    public Color highlightColor = Color.blue;

    /// <summary>
    /// Highlights all the tiles of the selected type
    /// </summary>
    public void highlightAll()
    {
        foreach (LevelTileController lt in FindObjectsOfType<LevelTileController>())
        {
            if (lt.LevelTile.Content == selectContent)
            {
                if (lt.cover)
                {
                    lt.cover.GetComponent<SpriteRenderer>().color = highlightColor;
                }
            }
        }
    }

    /// <summary>
    /// Reveals all the tiles of the selected type
    /// </summary>
    public void revealAll()
    {
        foreach (LevelTile lt in Keepers.Map.TileMap.getTiles(alt => alt.Content == selectContent))
        {
            if (lt.Content == selectContent)
            {
                Keepers.Player.RevealTile(lt.Position);
            }
        }
    }
}
