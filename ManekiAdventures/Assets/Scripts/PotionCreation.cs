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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (piece2 != null)
        {
            if (piece2.getAttribute() != attribute.empty)
            {
                ingredient2.text = piece2.getAttribute().ToString();
            }
            else if (piece2.getDiscriptor() != discriptor.empty)
            {
                ingredient2.text = piece2.getDiscriptor().ToString();
            }
        }
        if (piece1 != null)
        {
            if (piece1.getAttribute() != attribute.empty)
            {
                ingredient1.text = piece1.getAttribute().ToString();
            }
            else if (piece1.getDiscriptor() != discriptor.empty)
            {
                ingredient1.text = piece1.getDiscriptor().ToString();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (piece1 != null && piece2 != null)
            if (piece1.getAttribute() != attribute.empty && piece2.getDiscriptor() != discriptor.empty)
            {
                //This is where Cases will come in, avoid Potions that can't be made

                craftPotion();
            }
            else if (piece2.getAttribute() != attribute.empty && piece1.getDiscriptor() != discriptor.empty)
            {
                //This is where Cases will come in, avoid Potions that can't be made
                Item temp = piece1;
                piece1 = piece2;
                piece2 = temp;

                craftPotion();
            }
            else
            {
                Debug.LogError("These potion ingredents are bad and dumb and stupid, please stop being dumb and a smart ass and trying to break my system");
                piece1 = null;
                piece2 = null;
            }
        }
    }

    public void craftPotion()
    {
        Item potion = new Item();
        potion.name = piece1.getAttribute().ToString() + " " + piece2.getDiscriptor().ToString() + " Potion";
        potion.Attribute = piece1.getAttribute();
        potion.Discriptor = piece2.getDiscriptor();
        potion.icon = potionSprite;
        Inventory.instance.Add(potion);

        Inventory.instance.Remove(piece1);
        Inventory.instance.Remove(piece2);

        piece1 = null;
        piece2 = null;
        potionText.text = "";
    }

}
