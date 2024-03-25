using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextAsset mapData;
    [SerializeField] TextAsset decorationData;
    private Dictionary<Vector2Int, OverlayTile> overlay = new Dictionary<Vector2Int, OverlayTile>();
    private List<CharacterInfo> characters = new List<CharacterInfo>();
    private List<Vector3Int> forbidden = new List<Vector3Int>();

    private void Start()
    {
        //  Reads the file where the map is described and creates it.
        MapReader map = MapReader.CreateWithStringData(this.mapData.text);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        forbidden = display.RenderMapData(map);

        //  If there´s any decoration, it does the same as before.
        if (decorationData != null)
        {
            MapReader deco = MapReader.CreateWithStringData(decorationData.text);
            forbidden.AddRange(display.RenderDecoration(deco));
        }

        //  Creates the tiles where the characters are going to move
        MapManager manager = FindAnyObjectByType<MapManager>();
        if (manager != null) { overlay = manager.CreateOverlayTiles(forbidden); }
        else { Debug.Log("No existe MapManager"); }

        //  Create all the characters and place them in the map
        if (overlay != null)
        {
            CharacterDisplay character = FindObjectOfType<CharacterDisplay>();
            characters = character.CreateCharacters(overlay);
        }

        if (characters.Count != 0)
        {
            Mouse mouse = FindObjectOfType<Mouse>();
            mouse.SetCharacters(characters);
        }
        else { Debug.Log("No existen personajes"); }

        //  If it´s the tutorial level, makes it start
        GameObject tutorialObject = GameObject.FindGameObjectWithTag("Tutorial");
        if (tutorialObject != null)
        {
            Tutorial tutorial = tutorialObject.GetComponent<Tutorial>();

            tutorial.SetCharacters(characters);
            tutorial.SetOverlay(overlay);
        }
    }
}
