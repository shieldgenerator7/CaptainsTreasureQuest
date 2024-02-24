using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Settings
    public GestureSettings gestureSettings;

    //Managers
    public Managers managers;
    public LevelManager levelManager;
    private GestureManager gestureManager;

    // Start is called before the first frame update
    void Awake()
    {
        init();
        hookupDelegates();
    }

    void init()
    {
        gestureManager = new GestureManager(gestureSettings);
    }

    void hookupDelegates()
    {
        GestureProfile gpMineSweeper = createMineSweeperGestureProfile();
        gestureManager.addGestureProfile("MineSweeper", gpMineSweeper);
    }

    GestureProfile createMineSweeperGestureProfile()
    {
        //MineSweeper
        GestureProfile gpMineSweeper = new GestureProfile();
        //Tap
        gpMineSweeper.OnTap += (curMPWorld) =>
        {
            if (Managers.Camera.AutoMoving)
            {
                Managers.Camera.pinpoint();
                return;
            }
            LevelTile tile = levelManager.getTile(curMPWorld);
            Vector2 movePos = levelManager.getWorldPos(tile.Position);
            GameObject.FindFirstObjectByType<PieceController>().move(new Vector2Int((int)movePos.x, (int)movePos.y));
            levelManager.processTapGesture(curMPWorld);
            Managers.Camera.checkForAutomovement(curMPWorld);
        };
        //Hold
        gpMineSweeper.OnHold += (curMPWorld, holdTime, finished) =>
        {
            if (Managers.Camera.AutoMoving)
            {
                Managers.Camera.pinpoint();
                return;
            }
            levelManager.processHoldGesture(curMPWorld, finished);
            if (finished)
            {
                Managers.Camera.checkForAutomovement(curMPWorld);
            }
        };
        //Pinch
        gpMineSweeper.OnPinch += (adjustment) =>
        {
            Managers.Camera.adjustScalePoint(adjustment);
        };
        //CursorMove
        gpMineSweeper.OnCursorMove += (curMPWorld, show) =>
        {
            Managers.Effect.hideCursor();
            if (show)
            {
                LevelTile lt = levelManager.getTile(curMPWorld);
                if (lt == null || !lt.Walkable)
                {
                    return;
                }
                if (!lt.Revealed || levelManager.TileMap.getDetectedCount(lt.Position) > 0
                    || lt == levelManager.StartTile || (Managers.Player.completedMap() && lt == levelManager.XTile)
                    || (lt.Content == LevelTile.Contents.MAP)
                    )
                {
                    Managers.Effect.moveCursor(lt);
                }
            }
        };
        //
        return gpMineSweeper;
    }

    private void Update()
    {
        float time = Time.time;
        float deltaTime = Time.deltaTime;
        gestureManager.Update(time, deltaTime);
    }
}
