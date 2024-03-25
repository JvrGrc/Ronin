using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GameObject InventoryPanel;
    private Item[] itemList = new Item[5];
    private Slot[] islots = new Slot[5];
    private CharacterInfo charact;
    private Item item;


    private void Start()
    {

        charact = this.transform.parent.gameObject.GetComponent<CharacterInfo>();
        InventoryPanel = this.transform.GetChild(0).gameObject;
        for (int i = 0; i < 5; i++)
        {
            islots[i] = InventoryPanel.transform.GetChild(0).transform.GetChild(i).GetComponent<Slot>();
        }

        itemList[0] = Item.Instantiate((Item)Resources.Load("Items/Sushi"));
        itemList[1] = Item.Instantiate((Item)Resources.Load("Items/Té verde"));

        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] != null)
            {
                islots[i].SetItem(itemList[i]);

            }
        }
        this.GetComponent<Canvas>().enabled = false;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (charact.GetSelected())
            {

                ToggleInventory();
            }


        }
        if (!charact.GetSelected())
        {

            this.GetComponent<Canvas>().enabled = false;
        }

    }
    public void ToggleInventory()
    {

        if (this.GetComponent<Canvas>().enabled == false)
        {
            this.GetComponent<Canvas>().enabled = true;

        }
        else
        {
            this.GetComponent<Canvas>().enabled = false;

        }
    }

    public void CloseInventory()
    {
        this.GetComponent<Canvas>().enabled = false;
    }

    public bool Add(Item item)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                islots[i].SetItem(item);
                return true;
            }
        }
        return false;
    }
    public void UpdateSlotsUI()
    {
        for (int i = 0; i < islots.Length; i++)
        {
            islots[i].UpdateInventory();
        }
    }
    public void AddItem(Item item)
    {
        bool hasAdded = Add(item);

        if (hasAdded)
        {
            UpdateSlotsUI();
        }
        else
        {

        }
    }

    public void RemoveItem(int pos)
    {
        itemList[pos] = null;
        islots[pos].RemoveSlot();
    }

    public Item[] GetItemList()
    {
        return this.itemList;
    }
    public void UseItem(int pos)
    {
        itemList[pos].GetItemName();
        switch (itemList[pos].GetItemName())
        {
            case "Té Verde":
                if (charact.GetLife() == charact.GetMaxLife())
                {

                    break;
                }
                else
                {
                    charact.Heal();
                    RemoveItem(pos);
                    break;
                }
            case "Sushi":
                charact.Attackbuff();
                RemoveItem(pos);
                break;
        }
    }
}
