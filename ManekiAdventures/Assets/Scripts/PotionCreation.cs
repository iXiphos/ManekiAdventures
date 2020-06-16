using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCreation : MonoBehaviour
{

    public GameObject piece1, piece2;

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
                Debug.LogError("Hey, you made a potion");
            }
            else if (piece2.GetComponent<Attributes>() != null && piece1.GetComponent<Discriptor>() != null)
            {
                Debug.LogError("Hey, you made a potion");
            }
            else
            {
                Debug.LogError("These potion ingredents are bad and dumb and stupid, please stop being dumb and a smart ass and trying to break my system");
                piece1 = null;
                piece2 = null;
            }
        }
    }
}
