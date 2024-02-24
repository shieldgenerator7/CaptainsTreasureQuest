﻿using UnityEngine;
using System.Collections;
using System;

public class GestureProfile
{//2018-01-22: copied from Stonicorn.GestureProfile

    /// <summary>
    /// Called when this profile is set to the current one,
    /// or when the GestureManager switches off this profile to a different one
    /// </summary>
    public virtual void activate(bool active) {
        OnActivatedChanged?.Invoke(active);
    }
    public Action<bool> OnActivatedChanged;

    public virtual void processTapGesture(Vector3 curMPWorld)
    {        
        OnTap?.Invoke(curMPWorld);
    }
    public Action<Vector3> OnTap;

    public virtual void processHoldGesture(Vector3 curMPWorld, float holdTime, bool finished)
    {        
        OnHold?.Invoke(curMPWorld, holdTime, finished);
    }
    public Action<Vector3, float, bool> OnHold;

    public void processDragGesture(Vector3 origMPWorld, Vector3 curMPWorld)
    {
        OnDrag?.Invoke(origMPWorld, curMPWorld);
    }
    public Action<Vector3, Vector3> OnDrag;

    public virtual void processPinchGesture(int adjustment)
    {
        OnPinch?.Invoke(adjustment);
    }
    public Action<int> OnPinch;

    public virtual void processCursorMoveGesture(Vector3 curMPWorld, bool show)
    {
        OnCursorMove?.Invoke(curMPWorld, show);
    }
    public Action<Vector3, bool> OnCursorMove;
}
