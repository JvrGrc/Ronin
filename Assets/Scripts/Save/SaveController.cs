using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    GameData data = new GameData();
    string saveFile;
    //MouseController mouseController;
    [SerializeField]Mouse mouseControl;
    List<CharacterInfo> characterTurns = new List<CharacterInfo>();
    OverlayTile charactPosition;
    CharacterInfo loadCharacter;
    List<CharacterInfoData> saveCharacters = new List<CharacterInfoData>();
    int cont = 0;
    //List <InventoryData> saveInventory = new List<InventoryData>();

    private void Awake()
    {
        saveFile = Application.persistentDataPath + "/save.json";
    }

    public void LoadData()
    {
        characterTurns = mouseControl.GetCharacterTurns();
        characterSaved();
        deleteCharacters();
    }
  
    private void deleteCharacters()
    {
        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<GameData>(content);

            CharacterInfoData[] loadCharacters = data.charactersData.ToArray();
            if (loadCharacters.Length!= GameObject.Find("CharacterContainer").transform.childCount)
            {
                Debug.Log("No son iguales");
                
                for (int i = GameObject.Find("CharacterContainer").transform.childCount-1 ; i >=0; i--)
                {
                    for (int x = 0; x < loadCharacters.Length; x++)
                    {
                        Debug.Log(i+" "+x);
                        if (loadCharacters[x].name == GameObject.Find("CharacterContainer").transform.GetChild(i).GetComponent<CharacterInfo>().GetName())
                        {
                            cont++;
                        }
                    }
                   if (cont == 0)
                    {

                        mouseControl.deathCharacter(GameObject.Find("CharacterContainer").transform.GetChild(i).GetComponent<CharacterInfo>());
                       
                    }
                    cont = 0;
                }
            }
        }
    }
    private void characterSaved()
    {

        if (File.Exists(saveFile))
        {
            string content = File.ReadAllText(saveFile);
            data = JsonUtility.FromJson<GameData>(content);

            CharacterInfoData[] loadCharacters = data.charactersData.ToArray();

            for (int x = 0; x < loadCharacters.Length; x++)
            {
                for (int i = 0; i <= GameObject.Find("CharacterContainer").transform.childCount; i++)
                {
                    if (loadCharacters[x].name == GameObject.Find("CharacterContainer").transform.GetChild(i).GetComponent<CharacterInfo>().GetName())
                    {
                        loadCharacter = GameObject.Find("CharacterContainer").transform.GetChild(i).GetComponent<CharacterInfo>();
                        break;
                    }
                }
                for (int i = 0; i <= GameObject.Find("OverlayContainer").transform.childCount; i++)
                {
                    if (loadCharacters[x].tilePosition == GameObject.Find("OverlayContainer").transform.GetChild(i).GetComponent<OverlayTile>().GetGridLocation())
                    {
                        charactPosition = GameObject.Find("OverlayContainer").transform.GetChild(i).GetComponent<OverlayTile>();
                        break;
                    }
                }

                loadCharacter.SetCharacterStandingOnTile(charactPosition);
                loadCharacter.transform.position = charactPosition.transform.position;
                loadCharacter.SetLife(loadCharacters[x].life);
                loadCharacter.ChangeSelected(loadCharacters[x].selected);
                mouseControl.SetTurn(data.characterTurn);
            }
        }
    }

    public void loadScene()
    {
        string content = File.ReadAllText(saveFile);
        data = JsonUtility.FromJson<GameData>(content);
        PlayerPrefs.SetString("loadScene",data.sceneName );
        SceneManager.LoadScene(data.sceneName);
    }
    public void SaveData()
    {
        characterTurns = mouseControl.GetCharacterTurns();
        
        foreach(CharacterInfo character in characterTurns)
        {
            CharacterInfoData characterData = new CharacterInfoData()
            {
                tilePosition = character.GetCharacterStandingOnTile().GetGridLocation(),
                life = character.GetLife(),
                name = character.GetName(),
                selected = character.GetSelected(),
            };
            saveCharacters.Add(characterData);
            
        }
        GameData newData = new GameData()
        {
            sceneName = SceneManager.GetActiveScene().name,
            charactersData = saveCharacters,
            characterTurn = mouseControl.GetTurn()

        };

        string dataJson = JsonUtility.ToJson(newData);
        File.WriteAllText(saveFile, dataJson);
        Debug.Log("Guardado");
    }
}
