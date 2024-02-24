using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<Piece> pieces;

    //the tiles where this player can see enemy pieces
    private List<Vector2Int> mapVision = new List<Vector2Int>();
    //the tiles that this player has revealed
    private List<Vector2Int> mapRevealed = new List<Vector2Int>();
    //the tiles that this player has flagged as dangerous
    private List<Vector2Int> mapFlagged = new List<Vector2Int>();
    //the tiles that this player can detect the mine count of
    //i.e., the tiles this player can interact with in minesweeper
    private List<Vector2Int> mapDetect = new List<Vector2Int>();
    //the tiles that this player has dug for treasure at
    private List<Vector2Int> mapDigged = new List<Vector2Int>();

    #region MineSweeper

    //
    // Reveal Tile
    //

    public bool TileRevealed(Vector2Int pos)
        => mapRevealed.Contains(pos);
    public void RevealTile(Vector2Int pos, bool reveal = true)
    {
        //early exit: tile is flagged
        if (TileFlagged(pos)) { return; }

        //reveal tile
        bool changed = mapRevealed.Include(pos, reveal);

        //delegate
        if (changed)
        {
            OnRevealPosition?.Invoke(pos, reveal);
        }
    }
    public Action<Vector2Int, bool> OnRevealPosition;

    //
    // Flag Tile
    //

    public bool TileFlagged(Vector2Int pos) 
        => mapFlagged.Contains(pos);


    public void FlagTile(Vector2Int pos, bool flag)
    {
        //early exit: tile is revealed
        if (TileRevealed(pos)) { return; }

        //flag tile
        bool changed = mapFlagged.Include(pos, flag);

        //delegate
        if (changed)
        {
            OnFlagTile?.Invoke(pos, flag);
        }
    }
    public Action<Vector2Int, bool> OnFlagTile;

    //
    // Dig Tile
    //

    public Action<Vector2Int> OnDigTile;
    #endregion
}
