using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePiece
{
    public Piece piece { get; private set; }

    public Vector2Int _position;
    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPositionChanged?.Invoke(_position);
        }
    }
    public Action<Vector2Int> OnPositionChanged;

    public GamePiece(Piece piece)
    {
        this.piece = piece;
    }

    public bool canMove(Vector2Int pos)
    {
        Vector2Int dir = pos - _position;
        return piece.movePattern.allowedMoves.Contains(dir);
    }

    public List<Vector2Int> Detects =>
        piece.detectPattern.allowedMoves
        .ConvertAll(v => v + _position);

}
