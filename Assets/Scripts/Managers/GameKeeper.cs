using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeeper
{
    private Game game;
    public Game Game => game;

    public void newGame(TileMap map)
    {
        game = new Game();
        game.startGame(map);
    }
}
