using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*  IA Explanation
 * 
 *  The character checks which enemy is the closest too him and, if it is close enough, attacks and if not, he moves closer.
 *   
 */

public class EnemyAI : MonoBehaviour
{

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;
    private List<OverlayTile> attackTiles;
    private List<OverlayTile> rangeTiles;
    private Mouse mouse;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();
        attackTiles = new List<OverlayTile>();
        List<CharacterInfo> listEnemies = new List<CharacterInfo>();
    }

    public void Play(List<CharacterInfo> players, CharacterInfo character, Mouse m)
    {
        mouse = m;
        List<CharacterInfo> listEnemies = new List<CharacterInfo>();
        List<CharacterInfo> listAllies = new List<CharacterInfo>();
        foreach (CharacterInfo c in players)
        {
            if (!c.IsEnemy()) listEnemies.Add(c);
            else listAllies.Add(c);
        }

        //  I get the tile where the character is standing
        OverlayTile myTile = character.GetCharacterStandingOnTile();

        //  Fill the list with the occupied tiles that are in the attack range of our character
        attackTiles = rangeFinder.GetTilesInAtackRange(myTile.grid2DLocation, character.GetDistance()).Intersect(MapManager.Instance.GetOccupiedTiles()).ToList();

        //  Remove the tiles where the allies are from the attackTiles list
        foreach (CharacterInfo c in listAllies)
        {
            attackTiles.Remove(c.GetCharacterStandingOnTile());
        }

        if (attackTiles.Count > 0) Attack(listEnemies);
        else Move(character, listEnemies);

    }

    private void Move(CharacterInfo character, List<CharacterInfo> listEnemies)
    {
        //  Get the tiles where the character could move based on his movement range
        rangeTiles = rangeFinder.GetTilesInRange(character.GetCharacterStandingOnTile().grid2DLocation, character.GetMovement());
        rangeTiles.Remove(character.GetCharacterStandingOnTile());

        float finalDistance = -1;
        OverlayTile targetTile = null;
        foreach (CharacterInfo c in listEnemies)
        {
            foreach (OverlayTile o in rangeTiles)
            {
                float distance = Vector2.Distance(c.GetCharacterStandingOnTile().grid2DLocation, o.grid2DLocation);

                // The first tile is going to be the first distance reference
                if (finalDistance == -1) { finalDistance = distance; targetTile = o; }

                //  If the distance between tiles is less than before, it changes to the new distance and tile
                else if (distance < finalDistance) { finalDistance = distance; targetTile = o; }
            }
        }

        mouse.SetPath(pathFinder.FindPath(character.GetCharacterStandingOnTile(), targetTile, rangeTiles));
        mouse.SetEnemyMoving(true);
    }

    private void Attack(List<CharacterInfo> listEnemies)
    {
        CharacterInfo target = null;
        foreach (OverlayTile tile in attackTiles)
        {
            foreach (CharacterInfo c in listEnemies)
            {
                if (c.GetCharacterStandingOnTile().Equals(tile))
                {
                    if (target == null) target = c;
                    else if (target.GetLife() > c.GetLife()) target = c;
                }
            }
        }
        mouse.Attack(target);
    }

    public void PlayOld(List<CharacterInfo> players, int turn, Mouse mouse)
    {
        //  Create a list with his allies
        List<CharacterInfo> list = new List<CharacterInfo>();
        foreach (CharacterInfo character in players)
        {
            if (!character.IsEnemy()) list.Add(character);
        }

        //  Gets the tiles that are in his movement range and are occupied by his enemies
        OverlayTile attackTile = players[turn].GetCharacterStandingOnTile();
        mouse.SetAttackTiles(mouse.GetRangeFInder().GetTilesInAtackRange(new Vector2Int(attackTile.GetGridLocation().x, attackTile.GetGridLocation().y), players[turn].GetDistance()));
        mouse.SetAttackTiles(mouse.GetAttackTiles().Intersect(MapManager.Instance.GetOccupiedTiles()).ToList());
        foreach (CharacterInfo character in players)
        {
            if (character.IsEnemy())
            {
                mouse.GetAttackTiles().Remove(character.GetCharacterStandingOnTile());
            }
        }

        //  If there is any enemy in the atack range
        if (mouse.GetAttackTiles().Count > 0)
        {
            AttackOld(mouse, list);
        }
        //  Search for the closest enemy an moves closer
        else
        {
            MoveOld(players[turn], mouse, list);
        }
    }

    private void MoveOld(CharacterInfo player, Mouse mouse, List<CharacterInfo> list)
    {

        //  Get the tiles where the character could move by his movement range
        mouse.SetRangeTiles(mouse.GetRangeFInder().GetTilesInRange(player.GetCharacterStandingOnTile().grid2DLocation, player.GetMovement()));

        //  Searchs for the closest tile to a enemy
        float distance = 100;
        OverlayTile nearestTile = null;
        foreach (CharacterInfo character in list)
        {
            foreach (OverlayTile tile in mouse.GetRangeTiles())
            {
                float d = Vector2Int.Distance(character.GetCharacterStandingOnTile().grid2DLocation, tile.grid2DLocation);
                if (d < distance)
                {
                    distance = d;
                    nearestTile = tile;
                }
            }
        }

        //  Moves to the selected tile
        mouse.SetPath(mouse.GetPathFinder().FindPath(player.GetCharacterStandingOnTile(), nearestTile, mouse.GetRangeTiles()));
        mouse.SetEnemyMoving(true);
    }

    private static void AttackOld(Mouse mouse, List<CharacterInfo> list)
    {
        CharacterInfo atacado = null;
        foreach (OverlayTile tile in mouse.GetAttackTiles())
        {
            foreach (CharacterInfo c in list)
            {
                if (tile.Equals(c.GetCharacterStandingOnTile()))
                {
                    if (atacado == null) atacado = c;
                    else if (atacado.GetLife() > c.GetLife()) atacado = c;      //  Search for the enemy with less life
                }
            }
        }
        mouse.Attack(atacado);
        mouse.ChangeTurn();
    }
}