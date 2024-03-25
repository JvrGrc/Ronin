using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    private CharacterInfo character;
    private StatusMenu statusMenu;
    private Canvas canvasStatus;
    private Button status;
    private Button inventory;

    private void Start()
    {
        statusMenu = FindAnyObjectByType<StatusMenu>();
        canvasStatus = statusMenu.GetComponentInParent<Canvas>();
        Button[] buttons = gameObject.transform.GetChild(0).transform.GetComponentsInChildren<Button>();

        status = buttons[0];
        inventory = buttons[1];

        status.onClick.AddListener(ShowStatus);
        inventory.onClick.AddListener(ShowInventory);
    }

    public void SetCharacter(CharacterInfo character_)
    {
        character = character_;

        //  Shows the inventory option only if the character is selected.
        inventory.gameObject.SetActive(character.GetSelected());
    }

    public void ShowStatus()
    {
        if (!canvasStatus.enabled || !character.Equals(statusMenu.GetCharacter()))
        {
            statusMenu.SetCharacterStatsWithStats(character);
            canvasStatus.enabled = true;
            character.gameObject.transform.GetChild(1).GetComponent<Canvas>().enabled = false;
        }
        else { canvasStatus.enabled = false; }
    }

    public void ShowInventory() 
    {
        character.gameObject.transform.GetChild(1).GetComponent<Inventory>().ToggleInventory();
        if (canvasStatus.enabled) { canvasStatus.enabled = false; }
    }
}