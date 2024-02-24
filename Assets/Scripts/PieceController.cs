using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GamePiece piece;
    public Piece _piece;

    public float moveSpeed = 2;//TODO: move this into an attributes file
    public float arriveThreshold = 0.1f;//TODO: move this into an attributes file

    private Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        //if (piece == null)
        //{
            piece = new GamePiece(_piece);
        //}
        transform.position = (Vector2)piece.Position;
        targetPos = piece.Position;
        piece.OnPositionChanged += (pos) => targetPos = Keepers.Map.getWorldPos(pos);
        //
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != targetPos)
        {
            transform.position += (Vector3)(targetPos - (Vector2)transform.position).normalized * moveSpeed * Time.deltaTime;
            float dist = Vector2.Distance(transform.position, targetPos);
            if (dist <= arriveThreshold)
            {
                transform.position = targetPos;
            }
        }
    }

    public void teleport(Vector2Int pos)
    {
        piece.Position = pos;
        transform.position = Keepers.Map.getWorldPos(pos);
    }

    public bool move(Vector2Int pos)
    {
        if (!piece.canMove(pos))
        {
            return false;
        }
        piece.Position = pos;
        return true;
    }    
}
