using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [SerializeField] GameObject dialogObject;
    [SerializeField] Image attackExplanaition;
    [SerializeField] Canvas status;
    [SerializeField] Tilemap map;
    [SerializeField] TileBase newTile;
    [SerializeField] TileBase doorIzq;
    [SerializeField] GameObject character;
    [SerializeField] TextAsset text;

    private List<CharacterInfo> characters;
    private CharacterInfo mainCharacter;
    private Dialog dialog;
    private Mouse mouse;
    private CharacterContainer container;

    private Dictionary<Vector2Int, OverlayTile> overlay;

    private int phase;
    private bool entered;
    private bool attacked;
    private bool aux;
    private int rigth;
    private bool created;
    private void Start()
    {
        dialog = dialogObject.GetComponent<Dialog>();
        mouse = GameObject.FindGameObjectWithTag("Control").GetComponent<Mouse>();
        characters = new List<CharacterInfo>();
        container = FindAnyObjectByType<CharacterContainer>();
        phase = 0;
        entered = false;
        attacked = false;
        aux = false;
        rigth = 0;
        created = false;
    }

    public void Update()
    {
        if (container.GetAllCharacters().Length == 1 && attacked)
        {
            phase = 6;
            attacked = false;
        }

        switch (phase)
        {
            case 0:
                if (!entered)
                {
                    StartCoroutine(ReadTutorial(0, 3, 1));
                }
                break;
            case 1:
                if (!entered && !mouse.CharacterMoving() && (mainCharacter.GetInitialPos() != mainCharacter.GetCharacterStandingOnTile().grid2DLocation))
                {
                    StartCoroutine(ReadTutorial(4, 5, 2));
                }
                break;
            case 2:
                if (!entered)
                {
                    foreach (CharacterInfo character in container.GetAllCharacters())
                    {
                        if (character.GetLife() < character.GetMaxLife()) { attacked = true; break; }
                        if (attacked) { attackExplanaition.gameObject.SetActive(true); StartCoroutine(ReadTutorial(6, 9, 3)); mouse.enabled = false; }
                    }
                }
                break;
            case 3:
                //  If the image and the dialog are not active
                if (attackExplanaition.gameObject.activeSelf == false && dialog.transform.GetChild(0).gameObject.activeSelf == false)
                {
                    mouse.enabled = true;
                    StartCoroutine(ReadTutorial(10, 12, 4));

                    //  The main character is given a Katana.
                    if (mainCharacter.GetWeaponObj() == null)
                    {
                        GameObject o = Instantiate((GameObject)Resources.Load("Prefabs/Weapon/Katana"));
                        o.transform.SetParent(mainCharacter.gameObject.transform);
                        mainCharacter.SetWeaponObj(o);
                    }
                }
                break;
            case 4:
                if (container.GetAllCharacters().Length < characters.Count && !aux)
                {
                    StartCoroutine(ReadTutorial(13, 13, 4));     // The user is shown ho to see the stats
                    aux = true;
                }
                if (Input.GetMouseButtonDown(1)) rigth++;

                if (rigth > 0 && status.enabled == false)
                {
                    phase = 5;
                    rigth = 0;
                }
                break;
            case 5:
                if (Input.GetMouseButtonDown(1)) rigth++;
                if (rigth > 0 && status.enabled == false)
                {
                    StartCoroutine(ReadTutorial(14, 14, 6));     // The user is shown ho to see the inventory
                    rigth = 0;
                }
                break;
            case 6:
                //  If the user kills all the enemies
                if (container.GetAllCharacters().Length == 1)
                    StartCoroutine(ReadTutorial(15, 16, 7));
                break;
            case 7:
                Transform g = dialog.transform.Find("Panel");
                if (g.gameObject.activeSelf == false)
                {
                    map.SetTile(new Vector3Int(5, 0, 0), newTile);
                    map.SetTile(new Vector3Int(4, 0, 0), doorIzq);
                    CreateCharacter();
                    StartCoroutine(ReadTutorial(17, 19, 8));
                }
                break;
            case 8:
                Transform g1 = dialog.transform.Find("Panel");
                if (g1.gameObject.activeSelf == false)
                {
                    //Change to Level 1
                    SceneManager.LoadScene("TodoJunto");
                }
                break;
        }
    }

    IEnumerator ReadTutorial(int start, int finish, int nextPhase)
    {
        entered = true;
        yield return new WaitForSeconds(0.2f);
        dialogObject.SetActive(true);
        dialog.StartDialogFromTo(text, start, finish);
        if (nextPhase > 0)
        {
            phase = nextPhase;
            entered = false;
        }
    }

    public void SetCharacters(List<CharacterInfo> list)
    {
        this.characters = list;
        foreach (CharacterInfo character in characters)
        {
            if (!character.IsEnemy())
            {
                mainCharacter = character;
                break;
            }
        }
    }

    private void CreateCharacter()
    {
        if (!created)
        {
            CharacterContainer container = FindAnyObjectByType<CharacterContainer>();
            GameObject characterObj = Instantiate(character);
            CharacterInfo c = characterObj.GetComponent<CharacterInfo>();
            SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

            c.SetInitialPos(new Vector2Int(5, -1));
            c.SetMaxLife(40);
            c.SetLife(30);
            c.SetCharacterStandingOnTile(overlay[c.GetInitialPos()]);
            characterObj.transform.position = overlay[c.GetInitialPos()].transform.position;

            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Ninja2");
            spriteRenderer.sortingOrder = 1;

            if (container != null) { characterObj.transform.parent = container.transform; }

            created = true;
        }
    }

    public void SetOverlay(Dictionary<Vector2Int, OverlayTile> overlay1)
    {
        overlay = overlay1;
    }
}
