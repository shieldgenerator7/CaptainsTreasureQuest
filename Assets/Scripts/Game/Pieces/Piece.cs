using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece", menuName = "Piece/Piece")]
public class Piece : ScriptableObject
{
    [Header("Move Patterns")]
    public MovePattern movePattern;
    public MovePattern capturePattern;
    public MovePattern shootPattern;
    public MovePattern visionPattern;
    public MovePattern detectPattern;
    public MovePattern digPattern;

}
