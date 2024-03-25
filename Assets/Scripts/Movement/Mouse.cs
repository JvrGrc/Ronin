using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ArrowTranslator;

public class Mouse : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject dialog;
    [SerializeField] GameObject HUD;
    [SerializeField] string nextScene;
    [SerializeField] int cameraSpeed;
    [SerializeField] TextAsset text;

    private List<CharacterInfo> players;
    private int turn = 0;
    private bool isMoving;
    private Vector3 target;

    private RangeFinder rangeFinder;
    private List<OverlayTile> rangeFinderTiles;
    private List<OverlayTile> attackRangeTiles;

    private PathFinder pathFinder;
    private List<OverlayTile> path;
    private ArrowTranslator arrowTranslator;
    private StatusMenu menu;
    private Canvas menuParent;
    private Animator anim;
    private Dialog dialog_;

    private bool end = false;
    private bool enemyMoving = false;
    private float minDistance = 0.1f;

    public void SetCharacters(List<CharacterInfo> characters)
    {
        players = characters.OrderByDescending(c => c.GetSpeed()).ToList();

        players[turn].ChangeSelected(true);
        isMoving = false;

        rangeFinder = new RangeFinder();
        rangeFinderTiles = new List<OverlayTile>();
        attackRangeTiles = new List<OverlayTile>();
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        arrowTranslator = new ArrowTranslator();

        menu = FindAnyObjectByType<StatusMenu>();
        menu.SetCharacterStatsWithStats(players[turn]);
        menuParent = menu.GetComponentInParent<Canvas>();

    }
    private void Start()
    {
        if (PlayerPrefs.GetString("loadScene") == SceneManager.GetActiveScene().name)
        {
            SaveController sc = GameObject.FindAnyObjectByType<SaveController>().GetComponent<SaveController>();
            sc.LoadData();
        }
        dialog_ = dialog.transform.parent.GetComponent<Dialog>();
    }
    private void Update()
    {
        SetOrder();
        MoveCamera();
        if (!IsPause())     //  If the pause menu is not active and there´s no dialog open pausing the scene
        {
            if (end) { SceneManager.LoadScene(nextScene); }     //  This is used to show some dialog before going to the next scene
            if (!players[turn].IsEnemy())
            {
                if (!isMoving)
                {
                    PlayerMovementRange();
                    AtackRange();
                }

                RaycastHit2D? hit = GetFocusedOnTile();

                if (hit.HasValue)
                {
                    OverlayTile tile = ShowCursor(hit);

                    if (rangeFinderTiles.Contains(tile) && !isMoving)
                    {
                        path = pathFinder.FindPath(players[turn].GetCharacterStandingOnTile(), tile, rangeFinderTiles);

                        foreach (var item in rangeFinderTiles)
                        {
                            MapManager.Instance.GetMap()[item.grid2DLocation].SetSprite(ArrowDirection.None);
                        }

                        for (int i = 0; i < path.Count; i++)
                        {
                            var previousTile = i > 0 ? path[i - 1] : players[turn].GetCharacterStandingOnTile();
                            var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                            var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                            path[i].SetSprite(arrow);
                        }
                    }
                    else
                    {
                        ClearArrows();
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Vector2Int tilePressed = FindTilePressed(tile);
                        if (!tilePressed.Equals(new Vector2Int(-1, -1)))        //  If the tile pressed is valid
                        {
                            if (!IsCharacter(tilePressed))
                            {
                                if (!isMoving && TileInPath(tilePressed))
                                {
                                    isMoving = true;
                                }
                            }
                            else
                            {
                                foreach (OverlayTile overlay in attackRangeTiles)
                                {
                                    if (overlay.grid2DLocation.Equals(tilePressed))
                                    {
                                        if (players.Any(p => p.GetCharacterStandingOnTile().Equals(overlay)))
                                        {
                                            Attack(players.First(p => p.GetCharacterStandingOnTile().Equals(overlay)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Input.GetMouseButtonDown(1)) { CheckCanvas(FindTilePressed(tile)); }
                }

                if (isMoving)
                {
                    MoveAlongPath();
                }
            }
            else
            {
                if (!enemyMoving)
                {
                    if (Camera.main.transform.position == target)
                    {
                        GameObject g = players[turn].gameObject;
                        EnemyAI ai = g.GetComponent<EnemyAI>();
                        ai.Play(players, players[turn], this);
                    }
                }
                else
                {
                    MoveAlongPath();
                }
            }
        }
    }

    private void MoveCamera()
    {
        target = new Vector3(players[turn].transform.position.x, players[turn].transform.position.y, Camera.main.transform.position.z);

        float distance = Vector3.Distance(Camera.main.transform.position, target);

        if (distance <= minDistance)
        {
            Camera.main.transform.position = target;
        }
        else
        {
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, target, cameraSpeed * Time.deltaTime);
        }
    }

    public bool CharacterMoving() { return isMoving; }

    private bool IsPause()      //  Checks if the game is paused
    {
        if (dialog != null) return pauseCanvas.activeSelf || dialog.activeSelf;
        else return pauseCanvas.activeSelf;
    }

    public void Attack(CharacterInfo victim)       //  Attacks to the pressed character
    {
        if (victim.transform.position.x < players[turn].transform.position.x)
        {
            players[turn].transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            players[turn].transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (players[turn].GetSprite() == 5 || players[turn].GetSprite() == 6 || players[turn].GetSprite() == 7 ||
            players[turn].GetSprite() == 8 || players[turn].GetSprite() == 9 || players[turn].GetSprite() == 10 || players[turn].GetSprite() == 11)
        {
            anim = players[turn].gameObject.GetComponent<Animator>();
            anim.SetTrigger("TestTrigger");
        }
        players[turn].SetEnemigo(victim);
        players[turn].Attack();
        players[turn].SetEnemigo(null);

        if (victim.GetLife() == 0)
        {
            CheckLower(victim);
            players.Remove(victim);
            Destroy(victim.gameObject);
            CheckLast();
        }
        ChangeTurn();
    }

    private void CheckLower(CharacterInfo character)        //  It makes sure that the order is never broken
    {
        int cInt = 0;
        foreach (CharacterInfo c in players)
        {
            if (c.Equals(character)) { cInt = players.IndexOf(c); break; }
        }

        if (cInt < turn) { turn -= 1; }
    }

    private void CheckLast()        ////  Checks how many characters are alive
    {
        if (!nextScene.Equals("null"))
        {
            int enemyCount = 0;
            bool takeshiAlive = false;

            foreach (CharacterInfo c in players)
            {
                if (c.IsEnemy()) { enemyCount++; }
                else
                {
                    if (c.GetName().Equals("Takeshi")) takeshiAlive = true;
                }
            }
            //  If the only ally alive is the mainCharacter
            if (players.Count() - 1 == enemyCount && takeshiAlive && text != null)
            {
                dialog.SetActive(true);
                dialog_.StartDialogFromTo(text, 1, 5);
                end = true;
            }
            //  If all the characters alive are Enemies
            else if (players.All(c => c.IsEnemy()))
            {
                dialog.SetActive(true);
                dialog_.StartDialogFromTo(text, 8, 11);
                end = true;
            }
            //If all the characters alive are Allies
            else if (players.All(c => !c.IsEnemy()))
            {
                //  If Takeshi is alive
                if (takeshiAlive)
                {
                    dialog.SetActive(true);
                    dialog_.StartDialogFromTo(text, 14, 17);
                    end = true;
                }
                //  If Takeshi is dead
                else
                {
                    dialog.SetActive(true);
                    dialog_.StartDialogFromTo(text, 20, 24);
                    end = true;
                }
            }

        }
    }

    private void AtackRange()       //  Shows the player if there are any enemies close enough to attack
    {
        CharacterInfo character = players[turn];

        var tile = character.GetCharacterStandingOnTile();
        attackRangeTiles = rangeFinder.GetTilesInAtackRange(new Vector2Int(tile.GetGridLocation().x, tile.GetGridLocation().y), character.GetDistance());


        attackRangeTiles = attackRangeTiles.Intersect(MapManager.Instance.GetOccupiedTiles()).ToList(); //It gets only the tiles where there are enemies

        foreach (OverlayTile item in attackRangeTiles)
        {
            if (players[turn].GetCharacterStandingOnTile().grid2DLocation.Equals(item.grid2DLocation)) { attackRangeTiles.Remove(item); break; }
        }

        attackRangeTiles = DeleteTilesWithAllies(attackRangeTiles);

        foreach (var item in attackRangeTiles)
        {
            item.ShowTile1();
        }
    }

    private List<OverlayTile> DeleteTilesWithAllies(List<OverlayTile> rangeFinderTiles)     //  Delete the enemies that could be in the attack range
    {
        if (players[turn].IsEnemy())        //  If the attacker is an enemy it dleetes all the tiles occupied by enemies
        {
            foreach (CharacterInfo c in players)
            {
                if (c.IsEnemy())
                {
                    if (rangeFinderTiles.Contains(c.GetCharacterStandingOnTile()))
                        rangeFinderTiles.Remove(c.GetCharacterStandingOnTile());
                }
            }
            return rangeFinderTiles;
        }
        else                                //  If the attacker is an ally it dleetes all the tiles occupied by allies
        {
            foreach (CharacterInfo c in players)
            {
                if (!c.IsEnemy())
                {
                    if (rangeFinderTiles.Contains(c.GetCharacterStandingOnTile()))
                        rangeFinderTiles.Remove(c.GetCharacterStandingOnTile());
                }
            }
            return rangeFinderTiles;
        }
    }

    public void CheckCanvas(Vector2Int tile)       //   Checks the status of the canvas making it appear or not and changing the info
    {
        CharacterInfo c = null;
        foreach (CharacterInfo character in players)
        {
            if (character.GetCharacterStandingOnTile().grid2DLocation.Equals(tile))
            {
                c = character;
            }
        }
        if (c != null)      //  If it has been clicked on a character, the menu is displayed.
        {
            HUD.GetComponent<HUD>().OpenCharMenu(c);
            if (c != players[turn])
            {
                players[turn].gameObject.transform.GetChild(1).GetComponent<Inventory>().CloseInventory();
            }
        }
        else
        {
            HUD.GetComponent<HUD>().CloseCharMenu();
            menuParent.enabled = false;
            players[turn].gameObject.transform.GetChild(1).GetComponent<Inventory>().CloseInventory();
        }
    }

    public void ChangeTurn()       //Changes the turn to the next player
    {
        if (players != null)
        {
            players[turn].ChangeSelected(false);
            if (turn >= players.Count - 1) { turn = 0; } else { turn += 1; }
            players[turn].ChangeSelected(true);
        }
        if (players[turn].GetMovement() == 0) { ChangeTurn(); }     //  Especial line for the tutorial. It skips the enemies that cannot move

        menu.enabled = false;
    }

    private void MoveAlongPath()        //Moves the character to the next position
    {
        HUD.GetComponent<HUD>().CloseCharMenu();
        CharacterInfo character = players[turn];
        if (character.GetSprite() == 5 || character.GetSprite() == 6 || character.GetSprite() == 7 ||
            character.GetSprite() == 8 || character.GetSprite() == 9 || character.GetSprite() == 10 || character.GetSprite() == 11)
        {
            anim = character.gameObject.GetComponent<Animator>();
            anim.SetBool("Moving", true);
        }
        var step = 1 * Time.deltaTime;

        if (path[0].transform.position.x < character.transform.position.x)
        {
            character.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            character.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, 1);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0], character);
            path.RemoveAt(0);
        }
        if (path.Count == 0)
        {
            ClearArrows();
            HideRangeTiles();
            isMoving = false;
            enemyMoving = false;
            if (character.GetSprite() == 5 || character.GetSprite() == 6 || players[turn].GetSprite() == 7 ||
                players[turn].GetSprite() == 8 || players[turn].GetSprite() == 9 || players[turn].GetSprite() == 10 || players[turn].GetSprite() == 11)
            {
                anim.SetBool("Moving", false);
            }
            ChangeTurn();
        }
    }

    private void HideRangeTiles()       //  Hide the movement range tiles of the character
    {
        foreach (var tile in rangeFinderTiles)
        {
            tile.HideTile();
        }

        foreach (var tile in attackRangeTiles)
        {
            tile.HideTile();
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile, CharacterInfo character)     //  Place the character in the right place when it is close enough
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.SetCharacterStandingOnTile(tile);
    }

    private bool TileInPath(Vector2Int tile)        //  Checks if the tile pressed is inside the movement path
    {
        return rangeFinderTiles.Any(overlay => overlay.grid2DLocation.Equals(tile));
    }

    private bool IsCharacter(Vector2Int tile)       //  Checks if the pressed tile has a character or not.
    {
        return players.Any(character => character.GetCharacterStandingOnTile().grid2DLocation.Equals(tile));
    }

    private Vector2Int FindTilePressed(OverlayTile tile)        //  Gives back the position of the pressed tile
    {
        Dictionary<Vector2Int, OverlayTile> map = MapManager.Instance.GetMap();

        foreach (var item in map)
        {
            if (map[item.Key].Equals(tile))
            {
                return item.Key;
            }
        }
        return new Vector2Int(-1, -1);
    }

    private void ClearArrows()      //  Clears all the arrows printed on the OverlayTiles
    {
        foreach (var tile in rangeFinderTiles)
        {
            MapManager.Instance.GetMap()[tile.grid2DLocation].SetSprite(ArrowDirection.None);
        }
    }

    private OverlayTile ShowCursor(RaycastHit2D? hit)       //  Shows an special sprite on the tile where the mouse is located
    {
        OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
        cursor.transform.position = tile.transform.position;
        cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
        return tile;
    }

    private static RaycastHit2D? GetFocusedOnTile()     //  Gets the position of the mouse
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void PlayerMovementRange()      //  Shows the movement range that the character has
    {
        var tile = players[turn].GetCharacterStandingOnTile();

        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(tile.GetGridLocation().x, tile.GetGridLocation().y), players[turn].GetMovement());

        // Exclude tiles occupied by characters
        rangeFinderTiles = rangeFinderTiles.Except(MapManager.Instance.GetOccupiedTiles()).ToList();

        foreach (var item in rangeFinderTiles)
        {
            item.ShowTile();
        }
    }

    private void SetOrder()     //Orders the sprites.
    {
        foreach (CharacterInfo characterInfo in players) { SpriteRenderer r = characterInfo.GetComponent<SpriteRenderer>(); r.sortingOrder = 1; }
    }

    public List<CharacterInfo> GetCharacterTurns()      //  Public method to access to the character from another classes
    {

        return players;
    }
    public void SetTurn(int CharacterTurn)
    {
        this.turn = CharacterTurn;
    }
    public int GetTurn()
    {
        return turn;
    }
    public void deathCharacter(CharacterInfo dc)
    {
        players.Remove(dc);
        Destroy(dc.gameObject);
    }

    public void SetRangeTiles(List<OverlayTile> newRange) { rangeFinderTiles = newRange; }
    public List<OverlayTile> GetRangeTiles() { return rangeFinderTiles; }
    public void SetAttackTiles(List<OverlayTile> newAttack) { attackRangeTiles = newAttack; }
    public List<OverlayTile> GetAttackTiles() { return attackRangeTiles; }
    public void SetPath(List<OverlayTile> newPath) { path = newPath; }
    public List<OverlayTile> GetPath() { return path; }
    public void SetEnemyMoving(bool moving) { enemyMoving = moving; }
    public bool GetEnemyMoving() { return enemyMoving; }
    public RangeFinder GetRangeFInder() { return rangeFinder; }
    public PathFinder GetPathFinder() { return pathFinder; }
}