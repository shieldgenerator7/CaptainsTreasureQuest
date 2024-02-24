using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<GamePiece> pieces = new List<GamePiece>();

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

    #region Chess

    //
    // Pieces
    //

    public void addPiece(GamePiece piece)
    {
        this.pieces.Include(piece, true);
        this.updateDetectTiles();
        piece.OnPositionChanged += (pos) => { this.updateDetectTiles(); };
    }

    #endregion

    #region MineSweeper

    //
    // Detect Tile
    //

    private void updateDetectTiles()
    {
        mapDetect.Clear();
        pieces.ForEach(piece =>
        {
            mapDetect.AddRange(piece.Detects);
        });
    }

    public bool CanDetect(Vector2Int pos)
        => mapDetect.Contains(pos);

    //
    // Reveal Tile
    //

    public bool TileRevealed(Vector2Int pos)
        => mapRevealed.Contains(pos);
    public void RevealTile(Vector2Int pos, bool reveal = true)
    {
        //early exit: tile is flagged
        if (TileFlagged(pos)) { return; }
        //early exit: tile is outside detect range
        if (mapDetect.Count > 0 && !mapDetect.Contains(pos)) { return; }

        //reveal tile
        bool changed = mapRevealed.Include(pos, reveal);

        //delegate
        if (changed)
        {
            OnRevealTile?.Invoke(pos, reveal);
        }
    }
    public Action<Vector2Int, bool> OnRevealTile;

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
