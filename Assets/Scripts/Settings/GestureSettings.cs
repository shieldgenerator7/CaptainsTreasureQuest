using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GestureSettings",menuName ="Settings/Gesture")]
public class GestureSettings : ScriptableObject
{
    [Tooltip("How far from the original mouse position the current position has to be to count as a drag, " +
        "as a percent of the screen")]
    public float dragThresholdPercent = 20;
    [Tooltip("How long the tap has to be held to count as a hold (in seconds)")]
    public float holdThreshold = 0.1f;
    public float orthoZoomSpeed = 0.5f;
}
