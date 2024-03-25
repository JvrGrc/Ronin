using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Item item;
    [SerializeField] GameObject itemSprite;
    [SerializeField]GameObject itemDelete;
    private Button deletebuttton;
    [SerializeField] GameObject itemNameObject;
    [SerializeField] GameObject itemDescObject;
    [SerializeField] Text itemName;
    [SerializeField] Text itemDescription;
    [SerializeField] Button iconButton;
    [SerializeField] Inventory inventory;
    private int pos;

    private void Start()
    {
        
        //Creo los game objects
        itemSprite = this.transform.GetChild(0).gameObject;
        itemNameObject = this.transform.GetChild(1).gameObject;
        itemDescObject = this.transform.GetChild(2).gameObject;
        itemDelete = this.transform.GetChild(3).gameObject;
        
        //Les asigno los botones/texto
        deletebuttton = itemDelete.GetComponent<Button>();
        itemName = itemNameObject.GetComponent<Text>();
        itemDescription = itemDescObject.GetComponent<Text>();
        iconButton = itemSprite.GetComponent<Button>();
        //Llamo al inventario
        inventory = this.transform.parent.parent.parent.gameObject.GetComponent<Inventory>();
        //Añado Listener para los botones
        deletebuttton.onClick.AddListener(ActionDeletenButton);
        iconButton.onClick.AddListener(ActionIconButton);
        //Cojo el nombre del slot para saber su posicion
        pos=int.Parse(this.gameObject.name);
        UpdateInventory();
        
    }

    public void SetItem(Item item)
    {
        this.item= item;
    }

   public void UpdateInventory()
    {
        if (item != null)
        {
            
            itemSprite.GetComponent<Image>().sprite = item.GetItemIcon();
            itemSprite.GetComponent<Image>().enabled = true;
            itemDelete.GetComponent<Image>().enabled = true;

            itemName.text = item.GetItemName();
            itemDescription.text = item.GetItemDescription();

        }
        else
        {
            transform.GetComponent<Image>().enabled=false;
            itemSprite.GetComponent <Image>().enabled=false;
            itemDelete.GetComponent<Image>().enabled = false;
            itemName.text = null;
            itemDescription.text = null;
        }
    }
    public void RemoveSlot()
    {
        
        SetItem(null);
        UpdateInventory();
        
    }
    void ActionIconButton()
    {
        inventory.UseItem(pos);
    }
    void ActionDeletenButton()
    {
        inventory.RemoveItem(pos);
    }
}
