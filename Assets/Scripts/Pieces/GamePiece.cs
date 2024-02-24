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
}
