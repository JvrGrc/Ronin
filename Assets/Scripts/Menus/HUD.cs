using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private GameObject tt;
    private GameObject ally;
    private GameObject enemy;
    private Mouse mouseControl;
    private List<CharacterInfo> characters;
    [SerializeField] GameObject cm;
    [SerializeField] GameObject tracker;

    void Start()
    {
        mouseControl = GameObject.FindGameObjectWithTag("Control").GetComponent<Mouse>();
        tt = GameObject.FindGameObjectWithTag("TurnTracker");
        enemy = GameObject.FindGameObjectWithTag("UIEnemy");
        ally = GameObject.FindGameObjectWithTag("UIAlly");

    }

    void LateUpdate()
    {
        Turns();
        AlliesEnemies();
    }

    public void Turns()
    {
        characters = mouseControl.GetCharacterTurns();
        if (tt.transform.childCount != characters.Count)
        {

            Debug.Log("entra");
            Debug.Log(tt.transform.childCount);
            Debug.Log(characters.Count);
            for (int c = 0; c < tt.transform.childCount; c++)
            {
                Destroy(tt.transform.GetChild(c).gameObject);
            }

            for (int i = 0; i <= characters.Count - 1; i++)
            {

                GameObject track = Instantiate(tracker);
                Image turnImagen = track.GetComponent<RectTransform>().GetComponentInChildren<Canvas>().GetComponentInChildren<Image>().transform.GetChild(0).GetComponent<Image>();

                turnImagen.sprite = GetSprite(characters[i].GetSprite());
                track.GetComponent<RectTransform>().SetParent(tt.transform);
                if (characters[i].IsEnemy()) turnImagen.color = Color.red;
            };

        }

        for (int x = 0; x <= characters.Count - 1; x++)
        {
            Image background = tt.transform.GetChild(x).GetComponent<RectTransform>().GetComponentInChildren<Canvas>().GetComponentInChildren<Image>();

            if (characters[x].GetSelected())
            {
                background.enabled = true;
            }
            else
            {
                background.enabled = false;
            }
        };
    }

    public void AlliesEnemies()
    {
        characters = mouseControl.GetCharacterTurns();
        if (ally.transform.childCount == 1)
        {
            for (int i = 0; i <= characters.Count - 1; i++)
            {

                if (!characters[i].IsEnemy())
                {
                    GameObject imageAlly = new GameObject();
                    Image turnImagenAlly = imageAlly.transform.AddComponent<Image>();
                    imageAlly.GetComponent<RectTransform>().SetParent(ally.transform);
                    turnImagenAlly.sprite = GetSprite(characters[i].GetSprite());
                }
                else
                {
                    GameObject imageEnemy = new GameObject();
                    Image turnImagenEnemy = imageEnemy.transform.AddComponent<Image>();
                    imageEnemy.GetComponent<RectTransform>().SetParent(enemy.transform);
                    turnImagenEnemy.sprite = GetSprite(characters[i].GetSprite());
                }
            }
        }
    }
    public void ToggleAlly()
    {
        Animator animAlly = ally.gameObject.transform.parent.GetComponent<Animator>();
        if (animAlly != null)
        {
            bool open = animAlly.GetBool("OpenAllyUI");
            animAlly.SetBool("OpenAllyUI", !open);
        }
    }
    public void ToggleEnemy()
    {
        Animator animAlly = enemy.gameObject.transform.parent.GetComponent<Animator>();
        if (animAlly != null)
        {
            bool open = animAlly.GetBool("OpenEnemyUI");
            animAlly.SetBool("OpenEnemyUI", !open);
        }
    }
    public void OpenCharMenu(CharacterInfo character)
    {
        cm.GetComponent<CharacterMenu>().SetCharacter(character);
        cm.GetComponent<Canvas>().enabled = true;
    }

    public void CloseCharMenu()
    {
        cm.GetComponent<Canvas>().enabled = false;
    }


    private Sprite GetSprite(int sprite)
    {
        Sprite[] characSprite = Resources.LoadAll<Sprite>("Sprites/Characters");
        if (sprite == 4) return Resources.Load<Sprite>("Sprites/dojoEnemy");
        else if (sprite == 5) return Resources.Load<Sprite>("Sprites/samurai2");
        else if (sprite == 6) return Resources.Load<Sprite>("Sprites/samurai1");
        else if (sprite == 7) return Resources.Load<Sprite>("Sprites/Ninja2");
        else if (sprite == 8) return Resources.Load<Sprite>("Sprites/samurai1B");
        else if (sprite == 9) return Resources.Load<Sprite>("Sprites/Ninja2B");
        else if (sprite == 10) return Resources.Load<Sprite>("Sprites/Samurai2B");
        else if (sprite == 11) return Resources.Load<Sprite>("Sprites/Takeshi");
        else return characSprite[sprite];
    }
}
