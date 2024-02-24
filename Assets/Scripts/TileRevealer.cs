using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileRevealer : MonoBehaviour
{
    [Tooltip("How long between revealing tile layers")]
    public float revealDelay = 0.1f;

    private List<Vector2Int> tilesToReveal = new List<Vector2Int>();

    private float lastRevealTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (tilesToReveal.Count > 0)
        {
            if (Time.time > lastRevealTime + revealDelay)
            {
                lastRevealTime = Time.time;
                processReveal();
            }
        }
        else
        {
            enabled = false;
        }
    }

    public void revealTilesAround(Vector2Int pos)
    {
        tilesToReveal.Add(pos);
        enabled = true;
    }

    private void processReveal()
    {
        Player player = Keepers.Player;
        List<Vector2Int> revealNow = new List<Vector2Int>(tilesToReveal);
        List<Vector2Int> revealLater = new List<Vector2Int>();
        tilesToReveal.Clear();
        foreach (Vector2Int pos in revealNow)
        {
            if (!player.TileRevealed(pos))
            {
                player.RevealTile(pos, true);
            }
            //Surrounding tiles
            List<LevelTile> surroundingTiles = Keepers.Map.TileMap.getSurroundingLandTiles(pos);
            bool emptyAllAround = !surroundingTiles.Any(lt => lt.Detectable);
            if (emptyAllAround)
            {
                foreach (LevelTile slt in surroundingTiles)
                {
                    Vector2Int spos = slt.Position;
                    if (!player.TileRevealed(spos) && 
                        player.CanDetect(spos) &&
                        !revealNow.Contains(spos))
                    {
                        revealLater.Include(spos, true);
                    }
                }
            }
        }
        tilesToReveal.AddRange(revealLater);
    }
}
