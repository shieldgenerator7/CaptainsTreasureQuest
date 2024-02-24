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
        levelManager.mapKeeper = Keepers.Map;
    }

    void hookupDelegates()
    {
        //Gesture
        GestureProfile gpMineSweeper = createMineSweeperGestureProfile();
        gestureManager.addGestureProfile("MineSweeper", gpMineSweeper);

        //Level
        levelManager.OnStartPositionChanged += (pos) =>
        {
            PieceController piece = GameObject.FindFirstObjectByType<PieceController>();
            piece.teleport(pos);
        };

        //Keepers
        Keepers.Map.OnMapChanged += (map) => Keepers.Game.newGame(map);
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
            LevelTile tile = Keepers.Map.getTile(curMPWorld);
            GameObject.FindFirstObjectByType<PieceController>().move(tile.Position);
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
                LevelTile lt = Keepers.Map.getTile(curMPWorld);
                if (lt == null || !lt.Walkable)
                {
                    return;
                }
                if (!lt.Revealed || Keepers.Map.TileMap.getDetectedCount(lt.Position) > 0
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
