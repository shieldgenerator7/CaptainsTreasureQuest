using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Managers managers;
    public LevelManager levelManager;
    public GestureManager gestureManager;

    // Start is called before the first frame update
    void Awake()
    {
        hookupDelegates();
    }

    void hookupDelegates()
    {
    }
}
