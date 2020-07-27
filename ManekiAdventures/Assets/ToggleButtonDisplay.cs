using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author:  Wesley Elmer
// Date:    7/27/2020

// This script allows the color and text that a button displays to be changed to an alternate version
public class ToggleButtonDisplay : MonoBehaviour
{
    [Tooltip("The color the button will display when toggled")]
    public Color altColor;
    [Tooltip("The text the button will display when toggled.")]
    public string altText;

    Image buttonGraphic;
    Text buttonText;
    
    Color defaultColor;
    string defaultText;
    bool isAltState = false;
    string potionCraftingWindowName = "PotionCraftingWindow";

    
    // Start is called before the first frame update
    void Start()
    {
        buttonGraphic = gameObject.GetComponent<Image>();
        buttonText = gameObject.GetComponentInChildren<Text>();
        
        // save the default info of the button
        defaultColor = buttonGraphic.color;
        defaultText = buttonText.text;
    }

    // Change the button's state to it's default state (false), or alternate state (true)
    public void setButtonAltState(bool state)
    {
        if(state == true)
        {
            buttonGraphic.color = altColor;
            buttonText.text = altText;
            isAltState = true;
        }
        else
        {
            buttonGraphic.color = defaultColor;
            buttonText.text = defaultText;
            isAltState = false;
        }
    }

    // update the buttons state to match that of the crafting menu
    public void updateButtonAltState()
    {
        setButtonAltState(GameObject.Find(potionCraftingWindowName).activeSelf);
    }

    // Toggle the button to it's alternate state
    [ContextMenu("Toggle Button Alt State")]
    public void toggleAltState()
    {   
        if(isAltState)
        {
            buttonGraphic.color = defaultColor;
            buttonText.text = defaultText;
        }
        else
        {
            buttonGraphic.color = altColor;
            buttonText.text = altText;
        }

        isAltState = !isAltState;
    }


}
