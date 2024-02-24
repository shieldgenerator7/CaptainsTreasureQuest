using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
