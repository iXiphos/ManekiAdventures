using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Provides functions to update the text of the inventory Title
// I know that this is kind of jank but I'm short on time and VS on my laptop doesn't have unity library installed so I'm flying blind. 
// I promise I usually write super clean code - wes
public class InventoryTitleUpdater : MonoBehaviour
{
    [Tooltip("Text the inventory header will display if an empty slot is hovered over.")]
    public string neutralTitle = "Inventory";
    [Tooltip("When highlighting an item, the item's name will be displayed in this color")]
    public Color itemTextColor = Color.green;
    [Tooltip("When highlighting an item, the item's name will be displayed in this font style")]
    public FontStyle itemFontStyle = FontStyle.Normal;
    
    Text titleText;
    Color defaultTextColor;
    FontStyle defaultFontStyle;

    void Start()
    {
        titleText = gameObject.GetComponent<Text>();
        
        defaultTextColor = titleText.color;
        defaultFontStyle = titleText.fontStyle;
    }
    
    public void updateTitle(GameObject inventorySlotObj)
    {
        titleText.text = inventorySlotObj.GetComponent<InventorySlot>().getItemName();
        titleText.color = itemTextColor;
        titleText.fontStyle = itemFontStyle;
    }

    public void clearTitle()
    {
        titleText.text = neutralTitle;
        titleText.color = defaultTextColor;
        titleText.fontStyle = defaultFontStyle;
    }
}
