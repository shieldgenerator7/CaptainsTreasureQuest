using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MovePattern", menuName = "Piece/MovePattern")]
public class MovePattern : ScriptableObject
{
    //public List<MovePart> moveParts;
    public bool allowJump = false;
    public List<Vector2> allowedMoves;

    public bool getMove(int x, int y)
    {
        return allowedMoves.Any(v => v.x == x && v.y == y);
    }
    public void setMove(int x, int y, bool allowed)
    {
        if (allowed)
        {
            if (!allowedMoves.Any(v => v.x == x && v.y == y))
            {
                allowedMoves.Add(new Vector2(x, y));
            }
        }
        else
        {
            allowedMoves.RemoveAll(v => v.x == x && v.y == y);
        }
    }
}
