using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GamePiece piece;
    public Piece _piece;

    public float moveSpeed = 2;//TODO: move this into an attributes file

    private Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        if (piece == null)
        {
            piece = new GamePiece(_piece);
        }
        transform.position = (Vector2)piece.Position;
        targetPos = piece.Position;
        piece.OnPositionChanged += (pos) => targetPos = Keepers.Map.getWorldPos(pos);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != targetPos)
        {
            transform.position += (Vector3)(targetPos - (Vector2)transform.position).normalized * Time.deltaTime;
        }
    }

    public void teleport(Vector2Int pos)
    {
        piece.Position = pos;
    }

    public void move(Vector2Int pos)
    {
        if (canMove(pos))
        {
            piece.Position = pos;
        }
    }

    public bool canMove(Vector2Int pos)
    {
        Vector2Int dir = pos - piece.Position;
        return _piece.movePattern.allowedMoves.Contains(dir);
    }
}
