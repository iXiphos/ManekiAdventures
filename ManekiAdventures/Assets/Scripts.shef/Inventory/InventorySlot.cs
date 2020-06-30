using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    Item item;

    public void AddItem (Item newItem) //adds icon of item to inveotory
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true; //displays "X" button for each slot
    }

    public void ClearSlot() //deletes sprite of icon for each item removed
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() //removes item
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem() //button to use item in inventory
    {
        if (item != null)
        {
            item.Use();
        }
    }

}
