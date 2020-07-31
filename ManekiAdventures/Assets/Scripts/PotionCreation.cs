using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionCreation : MonoBehaviour
{

    public Item piece1, piece2;

    public GameObject basePotion;

    public Text ingredient1;

    public Text ingredient2;

    public Text potionText;

    public Sprite potionSprite;

    public Image craftingImage1;
    public Image craftingImage2;
    public Image craftingImage3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (piece2 != null)
        {
            if (piece2.getDiscriptor() != discriptor.empty)
            {

                ingredient2.text = piece2.getDiscriptor().ToString();
                craftingImage2.enabled = true;
                craftingImage2.sprite = piece2.icon;
            }
        }
        if (piece1 != null)
        {
            if (piece1.getAttribute() != attribute.empty)
            {
                ingredient1.text = piece1.getAttribute().ToString();
                craftingImage1.enabled = true;
                craftingImage1.sprite = piece1.icon;
            }
        }
        if(piece1 != null && piece2 != null)
        {
            potionText.text = piece1.getAttribute().ToString() + " " + piece2.getDiscriptor().ToString() + " Potion";
            craftingImage3.sprite = potionSprite;
            craftingImage3.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
            craftingImage3.enabled = true;
        }
        

    }

    public void craftPotion()
    {
        ingredient1.text = "";
        ingredient2.text = "";
        potionText.text = "";
        craftingImage1.enabled = false;
        craftingImage1.sprite = null;
        craftingImage2.enabled = false;
        craftingImage2.sprite = null;
        craftingImage3.enabled = false;
        craftingImage3.sprite = null;
        if (piece2.getAttribute() != attribute.empty && piece1.getDiscriptor() != discriptor.empty)
        {
            //This is where Cases will come in, avoid Potions that can't be made
            Item temp = piece1;
            piece1 = piece2;
            piece2 = temp;

            craftPotion();
        }
        Item potion = new Item
        {
            name = piece1.getAttribute().ToString() + " " + piece2.getDiscriptor().ToString() + " Potion",
            Attribute = piece1.getAttribute(),
            Discriptor = piece2.getDiscriptor(),
            icon = potionSprite
        };
        Inventory.instance.Add(potion);

        Inventory.instance.Remove(piece1);
        Inventory.instance.Remove(piece2);

        piece1 = null;
        piece2 = null;
        potionText.text = "";
    }

    public void returnObjectsToInventory()
    {
        if (piece1 != null)
        {
            Inventory.instance.Add(piece1);
            piece1 = null;
        }
        if (piece2 != null)
        {
            Inventory.instance.Add(piece2);
            piece2 = null;
        }
        ingredient1.text = "";
        ingredient2.text = "";
        potionText.text = "";
        craftingImage1.enabled = false;
        craftingImage1.sprite = null;
        craftingImage2.enabled = false;
        craftingImage2.sprite = null;
        craftingImage3.enabled = false;
        craftingImage3.sprite = null;
    }


    public void enableCraft()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
