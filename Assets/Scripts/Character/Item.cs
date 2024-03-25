using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject

{
    [SerializeField] string itemName = "New Item";
    [SerializeField] string itemDescription = "New Description";
    [SerializeField] Sprite Icon;

    [SerializeField] ItemType type;
    public enum ItemType
    {
        Default,
        Weapon,
        Potion
    }

    public Sprite GetItemIcon()
    {
        return this.Icon;
    }
    public string GetItemName()
    {
        return this.itemName;
    }
    public string GetItemDescription()
    {
        return this.itemDescription;
    }
}


