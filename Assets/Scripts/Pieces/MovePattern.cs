using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MovePattern", menuName = "Piece/MovePattern")]
public class MovePattern : ScriptableObject
{
    //public List<MovePart> moveParts;
    public bool allowJump = false;
    [SerializeField]
    public List<Vector2> allowedMoves = new List<Vector2>()
    {
        Vector2.zero
    };

    public int maxRange =>
        (allowedMoves.Count > 0)
            ? (int)allowedMoves.Max(v => Mathf.Max(v.x, v.y))
            : 0;

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
