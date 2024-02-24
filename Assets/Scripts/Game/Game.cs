using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public List<Player> players = new List<Player>();
    public TileMap map;

    public void startGame(TileMap map)
    {
        this.map = map;
        int playerCount = 2;
        while(players.Count < playerCount)
        {
            players.Add(new Player());
        }
    }

    public Player ActivePlayer => Player1;//TODO: switch between players

    public Player Player1 => players[0];
    public Player Player2 => players[1];
}
