using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePart : ScriptableObject
{
    public enum Type
    {
        ORTHOGONAL,
        VERTICAL,
        HORIZONTAL,
        DIAGONAL,
        DIAGONAL_RIGHT,//up to the right
        DIAGONAL_LEFT,//up to the left
        KNIGHT,
        RADIUS,
    }
    public Type type;

    public int rangeMin = 1;
    public int rangeMax = 8;
}
