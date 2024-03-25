using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject loadMenu;
    [SerializeField] string nextScene;
    [SerializeField] Button loadButton;
    SaveController saveController;

    private void Start()
    {
        string file = Application.persistentDataPath + "/save.json";
        if (File.Exists(file)) {
            loadButton.enabled = true;
            Text text = loadButton.GetComponentInChildren<Text>();
            text.color = Color.black;
        } 
        else {
            loadButton.enabled = false;
            Text text = loadButton.GetComponentInChildren<Text>();
            text.color = new Color(0, 0, 0, 0.3f);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(nextScene);
        PlayerPrefs.SetString("loadScene", "none");
    }
    public void OpenloadGame()
    {
        Debug.Log("pulsado");
        saveController.loadScene();
    }
    public void CloseLoadMenu()
    {
        loadMenu.SetActive(false);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    private void Awake()
    {
        saveController = GameObject.FindAnyObjectByType<SaveController>().GetComponent<SaveController>();
    }

}
