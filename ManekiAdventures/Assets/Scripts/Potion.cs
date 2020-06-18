using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [HideInInspector]
    public attribute pAttribute;

    [HideInInspector]
    public discriptor pDiscriptor;

    public int explostionSize;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
            
             
    }


    IEnumerator Explosion(GameObject coll)
    {
        yield return new WaitForSeconds(0.1f);
        switch (pAttribute)
        {
            case attribute.Friction:
                //Change Object it hits friction

                break;

            case attribute.Size:
                //Change the Size of the Object

                break;

            case attribute.Weight:
                //Change the Mass of the object

                break;

            case attribute.Location:
                //Do something at Location It Hits(Cordinates)

                break;
        }

    }

}
