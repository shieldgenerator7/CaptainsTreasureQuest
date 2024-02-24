using System;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public List<Piece> pieces;

    //the tiles where this player can see enemy pieces
    public List<LevelTile> visionTiles = new List<LevelTile>();
    //the tiles that this player has revealed
    public List<LevelTile> revealedTiles = new List<LevelTile>();
    //the tiles that this player has flagged as dangerous
    public List<LevelTile> flaggedTiles = new List<LevelTile>();
    //the tiles that this player can detect the mine count of
    public List<LevelTile> detectTiles = new List<LevelTile>();
    //the tiles that this player has dug for treasure at
    public List<LevelTile> dugTiles = new List<LevelTile>();

    #region MineSweeper

    //
    // Reveal Tile
    //

    public bool TileRevealed(LevelTile tile) =>
        (tile.Walkable)
            ? revealedTiles.Contains(tile)
            : true;
    public void RevealTile(LevelTile tile, bool reveal = true)
    {
        //early exit: tile is water
        if (!tile.Walkable) { return; }
        //early exit: tile is flagged
        if (TileFlagged(tile)) { return; }

        //reveal tile
        bool changed = revealedTiles.Include(tile, reveal);

        //delegate
        if (changed)
        {
            OnRevealTile?.Invoke(tile, reveal);
        }
    }
    public Action<LevelTile, bool> OnRevealTile;

    //
    // Flag Tile
    //

    public bool TileFlagged(LevelTile tile) =>
        (tile.Walkable)
            ? flaggedTiles.Contains(tile)
            : false;

    public void FlagTile(LevelTile tile, bool flag)
    {
        //early exit: tile is water
        if (!tile.Walkable) { return; }
        //early exit: tile is revealed
        if (TileRevealed(tile)) { return; }

        //flag tile
        bool changed = flaggedTiles.Include(tile, flag);

        //delegate
        if (changed)
        {
            OnFlagTile?.Invoke(tile, flag);
        }
    }
    public Action<LevelTile, bool> OnFlagTile;

    //
    // Dig Tile
    //

    public Action<LevelTile> OnDigTile;
    #endregion
}
