using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{//2018-01-02: copied from WolfSim.LevelManager

    [Header("Levels")]
    public List<Level> levels;

    [Header("Objects")]
    public GameObject frame;

    public GameObject levelTilePrefab;

    public MapKeeper mapKeeper;

    //
    // Runtime vars
    //

    private int currentLevelIndex = 0;
    public int LevelIndex
    {
        get { return currentLevelIndex; }
        set
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException("Value must be greater than 0. value: " + value);
            }
            currentLevelIndex = value % levels.Count;
        }
    }
    private Level Level
    {
        get { return levels[currentLevelIndex]; }
        set { currentLevelIndex = levels.IndexOf(value); }
    }

    private bool anyRevealed = false;//true if any tile has been revealed

    private bool usedFirstHoldFrame;

    private ItemDisplayer foundItem;
    public ItemDisplayer FoundItem
    {
        get { return foundItem; }
        set { foundItem = value; }
    }

    // Use this for initialization
    void Start()
    {
        //Initialization stuff
        generateLevel(Level);
        updateOrthographicSize();
    }
    public bool checkReset(Vector2 tapPos)
    {
        bool gameOver = false;
        if (Managers.Player.Alive)
        {
            if (tapOnObject(Managers.Start, tapPos))
            {
                if (Managers.Start.activeSelf)
                {
                    gameOver = true;
                    reset();
                }
            }
            else if (tapOnObject(Managers.End, tapPos))
            {
                if (Managers.Player.completedMap())
                {
                    if (mapKeeper.getTile(tapPos).Revealed)
                    {
                        gameOver = true;
                        reset(false);
                    }
                }
            }
        }
        else
        {
            gameOver = true;
            reset();
        }
        return gameOver;
    }
    public void reset(bool resetToBeginning = true)
    {
        if (mapKeeper.TileMap != null)
        {
            foreach (LevelTileController ltc in FindObjectsOfType<LevelTileController>())
            {
                Destroy(ltc.gameObject);
            }
        }

        //Clear extra generated sprites
        foreach (LevelGenerator lgen in Level.postRevealLevelGenerators)
        {
            lgen.clearGeneratedObjects();
        }

        //Move to next level
        if (resetToBeginning)
        {
            LevelIndex = 0;
        }
        else
        {
            LevelIndex++;
        }
        generateLevel(Level);

        //Reset runtime variables
        anyRevealed = false;
        Managers.Player.reset();
    }

    private LevelTileController getTileController(LevelTile lt)
    {
        return FindObjectsOfType<LevelTileController>().First(ltc => ltc.LevelTile == lt);
    }



    public LevelTile StartTile
        => mapKeeper.getTile(Managers.Start.transform.position);
    public LevelTile XTile
        => mapKeeper.getTile(FindObjectOfType<MapLineUpdater>().LastRevealedSpot);

    public bool tapOnObject(GameObject go, Vector2 tapPos)
    {
        return mapKeeper.getGridPos(go.transform.position) == mapKeeper.getGridPos(tapPos);
    }

    public int getDisplaySortingOrder(Vector2 pos)
    {
        return (int)((Level.gridHeight / 2 - pos.y) * 100);
    }
    public Vector2 randomPosition()
    {
        return new Vector2(
            Random.Range(-Level.gridWidth / 2, Level.gridWidth / 2) * 0.9f,
            Random.Range(-Level.gridHeight / 2, Level.gridHeight / 2) * 0.9f
            );
    }
    public bool inBounds(Vector2 pos)
    {
        return pos.x > -Level.gridWidth / 2 * 0.99f
            && pos.x < Level.gridWidth / 2 * 0.99f
            && pos.y > -Level.gridHeight / 2 * 0.99f
            && pos.y < Level.gridHeight / 2 * 0.99f;
    }

    private void generateLevel(Level level)
    {
        int width = level.gridWidth;
        int height = level.gridHeight;
        TileMap tileMap = new TileMap(width, height);

        //Generate level
        foreach (LevelGenerator lgen in level.levelGenerators)
        {
            lgen.generate(tileMap);
        }

        //Update MapKeeper
        mapKeeper.switchMap(tileMap, width, height);

        //Instantiate GameObjects
        tileMap.getTiles(tile => tile.Walkable)
            .ForEach(tile =>
            {
                GameObject go = GameObject.Instantiate(levelTilePrefab);
                go.transform.position = mapKeeper.getWorldPos(tile.x, tile.y);
                go.GetComponent<LevelTileController>().LevelTile = tile;
                go.transform.parent = transform;
            });
    }

    /// <summary>
    /// After the initial reveal, generate the mines and treasures,
    /// avoiding the "position to avoid"
    /// </summary>
    /// <param name="posToAvoid"></param>
    private void generateLevelPostTap(Vector2 posToAvoid)
    {
        int itaX = mapKeeper.getXIndex(posToAvoid);
        int itaY = mapKeeper.getYIndex(posToAvoid);
        TileMap tileMap = mapKeeper.TileMap;
        foreach (LevelGenerator lgen in Level.postStartLevelGenerators)
        {
            lgen.generatePostStart(tileMap, itaX, itaY);
        }

        Managers.Start.SetActive(true);
        Managers.Start.transform.position = mapKeeper.getWorldPos(itaX, itaY);
        OnStartPositionChanged?.Invoke(new Vector2Int(itaX, itaY));

    }
    public Action<Vector2Int> OnStartPositionChanged;

    private void generatePostItemReveal(LevelTile.Contents content)
    {
        TileMap tileMap = mapKeeper.TileMap;
        foreach (LevelGenerator lgen in Level.postRevealLevelGenerators)
        {
            lgen.generatePostReveal(tileMap, content);
        }
    }

    public void processTapGesture(Vector2 tapPos)
    {
        TileMap tileMap = mapKeeper.TileMap;
        if (foundItem && Managers.Player.Alive)
        {
            recalculateNumbers();
            LevelTile foundLT = foundItem.levelTile.LevelTile;
            if (foundLT.Content != LevelTile.Contents.MAP)
            {
                //Reveal the found LT
                revealTile(foundLT, true);
                //Reveal the tiles around the found LT
                foreach (LevelTile levelTile in tileMap.getSurroundingLandTiles(foundLT.Position))
                {
                    if (levelTile.Revealed)
                    {
                        revealTile(levelTile, true);
                    }
                }
                //Check if goals have been achieved
                if (Managers.Player.GoalAchieved)
                {
                    //Check if map has been completed
                    if (Managers.Player.completedMap())
                    {
                        //Go to latest revealed location
                        Managers.Camera.moveTo(
                            FindObjectOfType<MapLineUpdater>().LastRevealedSpot
                            );
                    }
                    else
                    {
                        //Go to start
                        Managers.Camera.moveTo(Managers.Start);
                    }
                }
            }
            else
            {
                //Check if map has been completed
                if (Managers.Player.completedMap())
                {
                    //Go to latest revealed location
                    Managers.Camera.moveTo(
                        FindObjectOfType<MapLineUpdater>().LastRevealedSpot
                        );
                }
            }
            foundItem.retire();
            foundItem = null;
            return;
        }
        if (checkReset(tapPos))
        {
            return;
        }
        LevelTile lt = mapKeeper.getTile(tapPos);
        if (lt != null && lt.Walkable)
        {
            //If it's revealed
            if (lt.Revealed)
            {
                //Auto-Reveal
                //If the count of surrounding flags equals
                //the count of surrounding trap tiles,
                int itemCount = tileMap.getDetectedCount(lt.Position);
                if (!lt.Detectable && lt.Content != LevelTile.Contents.MAP &&
                    tileMap.getAdjacentFlagCount(lt.Position) == itemCount)
                {
                    //Reveal the surrounding non-flagged tiles
                    foreach (LevelTile neighbor in tileMap.getSurroundingLandTiles(lt.Position))
                    {
                        if (!neighbor.Flagged && !neighbor.Revealed)
                        {
                            if (neighbor.Content == LevelTile.Contents.TRAP)
                            {
                                Managers.Player.takeHit();
                            }
                            if (neighbor.Content == LevelTile.Contents.TREASURE)
                            {
                                Managers.Player.findTrophy();
                            }
                            revealTile(neighbor);
                        }
                    }
                    if (!Managers.Player.Alive)
                    {
                        revealBoard();
                    }
                }
                //Auto-Flag
                //If the count of surrounding unrevealed tiles equals
                //the count of surrounding trap tiles,
                if (!lt.Detectable && lt.Content != LevelTile.Contents.MAP &&
                    tileMap.getAdjacentRevealedCount(lt.Position, true) == itemCount)
                {
                    //Flag the surrounding non-revealed tiles
                    foreach (LevelTile neighbor in tileMap.getSurroundingLandTiles(lt.Position))
                    {
                        if (!neighbor.Flagged && !neighbor.Revealed)
                        {
                            //Flag it
                            processFlagGesture(mapKeeper.getPosition(neighbor));
                        }
                    }
                }
            }
            //If it's not flagged
            if (!lt.Flagged)
            {
                if (!anyRevealed)
                {
                    generateLevelPostTap(tapPos);
                    anyRevealed = true;
                }
                if ((!lt.Revealed) || tileMap.getDetectedCount(lt.Position) > 0)
                {
                    Managers.Effect.highlightChange(lt);
                }
                LevelTile.Contents revealedItem = LevelTile.Contents.NONE;
                bool shouldRevealBoard = false;
                bool prevRevealed = lt.Revealed;
                if (lt.Content == LevelTile.Contents.TRAP)
                {
                    revealedItem = LevelTile.Contents.TRAP;
                    if (!Managers.Player.takeHit())
                    {
                        shouldRevealBoard = true;
                    }
                }
                if (lt.Content == LevelTile.Contents.TREASURE)
                {
                    revealedItem = LevelTile.Contents.TREASURE;
                    Managers.Player.findTrophy();
                }
                if (revealedItem == LevelTile.Contents.TRAP || revealedItem == LevelTile.Contents.TREASURE)
                {
                    lt.Revealed = true;
                    Managers.Effect.highlightChange(lt);
                    if (shouldRevealBoard)
                    {
                        revealBoard();
                    }
                    generatePostItemReveal(revealedItem);
                }
                else
                {
                    revealTile(lt);
                }
                if (lt.Content == LevelTile.Contents.MAP)
                {
                    //if it's already been revealed
                    //but not activated yet
                    if (prevRevealed)
                    {
                        Managers.Effect.highlightChange(lt);
                        lt.Content = LevelTile.Contents.NONE;
                        Managers.Player.MapFoundCount++;
                        getTileController(lt).contentsSR.gameObject.AddComponent<ItemDisplayer>();
                        generatePostItemReveal(LevelTile.Contents.MAP);
                    }
                }
            }
        }
    }
    public void processFlagGesture(Vector2 flagPos)
    {
        if (checkReset(flagPos))
        {
            return;
        }
        LevelTile lt = mapKeeper.getTile(flagPos);
        if (!lt.Revealed)
        {
            lt.Flagged = !lt.Flagged;
            Managers.Effect.highlightChange(lt);
            //Update flag counters (fc)
            foreach (LevelTile fc in mapKeeper.TileMap.getSurroundingLandTiles(lt.Position))
            {
                if (fc.Revealed)
                {
                    getTileController(fc).numberDisplayer.displayNumber();
                }
            }
        }
    }
    public void processHoldGesture(Vector2 holdPos, bool finished)
    {
        LevelTile lt = mapKeeper.getTile(holdPos);
        if (lt != null)
        {
            //don't process empty spaces
            return;
        }
        if (!usedFirstHoldFrame)
        {
            usedFirstHoldFrame = true;
            processFlagGesture(holdPos);
            Vibration.Vibrate(75);
            if (lt.Revealed)
            {
                frame.SetActive(true);
            }
            else
            {
                Managers.Effect.highlightChange(lt);
            }
        }
        frame.transform.position = mapKeeper.getPosition(lt);
        if (finished)
        {
            usedFirstHoldFrame = false;
            frame.SetActive(false);
        }
    }

    private void revealTile(LevelTile lt, bool forceReveal = false)
    {
        if ((!lt.Revealed || forceReveal) && !lt.Flagged)
        {
            Managers.TileRevealer.revealTilesAround(lt);
        }
    }

    /// <summary>
    /// Reveals the important tiles of the board, namely the treasures
    /// </summary>
    private void revealBoard()
    {
        foreach (LevelTile lt in mapKeeper.TileMap.getTiles(alt => !alt.Revealed))
        {
            if (lt.Walkable && !lt.Revealed)
            {
                if (lt.Content == LevelTile.Contents.TREASURE
                    || lt.Content == LevelTile.Contents.TRAP
                    || lt.Content == LevelTile.Contents.MAP)
                {
                    lt.Revealed = true;
                    Managers.Effect.highlightChange(lt);
                }
            }
        }
    }

    /// <summary>
    /// Tells all the counter sprites to update after the board changes
    /// </summary>
    void recalculateNumbers()
    {
        foreach (NumberDisplayer nd in FindObjectsOfType<NumberDisplayer>())
        {
            nd.displayNumber();
        }
    }

    public void updateOrthographicSize()
    {
        while (true)//loop until broken out of
        {
            Vector2 screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))
                - Camera.main.ScreenToWorldPoint(Vector2.zero);
            if (screenSizeWorld.x > Level.gridWidth && screenSizeWorld.y > Level.gridHeight)
            {
                break;//all good here
            }
            Camera.main.orthographicSize++;
        }
    }
}
