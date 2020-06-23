using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCreation : MonoBehaviour
{

    public GameObject piece1, piece2;

    public GameObject basePotion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (piece1.GetComponent<Attributes>() != null && piece2.GetComponent<Discriptor>() != null)
            {
                //This is where Cases will come in, avoid Potions that can't be made

                craftPotion();
            }
            else if (piece2.GetComponent<Attributes>() != null && piece1.GetComponent<Discriptor>() != null)
            {
                //This is where Cases will come in, avoid Potions that can't be made
                GameObject temp = piece1;
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

    void craftPotion()
    {
        GameObject potion = Instantiate(basePotion, new Vector3(0,0,0), Quaternion.identity);
        potion.GetComponent<Potion>().pAttribute = piece1.GetComponent<Attributes>().attributeWord;
        potion.GetComponent<Potion>().pDiscriptor = piece2.GetComponent<Discriptor>().discriptorWord;
        potion.name = potion.GetComponent<Potion>().pDiscriptor.ToString() + " " + potion.GetComponent<Potion>().pAttribute.ToString() + " Potion";

        piece1 = null;
        piece2 = null;
    }

}
