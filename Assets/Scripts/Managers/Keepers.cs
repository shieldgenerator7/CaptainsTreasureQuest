using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Keepers
{
    static MapKeeper mapKeeper;
    public static MapKeeper Map
    {
        get
        {
            if (mapKeeper == null)
            {
                mapKeeper = new MapKeeper();
            }
            return mapKeeper;
        }
    }

    static GameKeeper gameKeeper;
    public static GameKeeper Game
    {
        get
        {
            if (gameKeeper  == null)
            {
                gameKeeper = new GameKeeper();
            }
            return gameKeeper;
        }
    }

    public static Player Player => Game.Game.ActivePlayer;
}
