using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [HideInInspector]
    public attribute pAttribute;

    [HideInInspector]
    public discriptor pDiscriptor;

    public int explosionSize;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
            
             
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            StartCoroutine(Explosion(collision.gameObject));
        }
    }


    IEnumerator Explosion(GameObject coll)
    {
        yield return new WaitForSeconds(0.1f);
        switch (pAttribute)
        {
            case attribute.Friction:
                //Change Object it hits friction
                if (pDiscriptor == discriptor.Increases)
                {
                    coll.GetComponent<PhysicMaterial>().dynamicFriction = 0.9f;
                }
                else if (pDiscriptor == discriptor.Decreases)
                {
                    coll.GetComponent<PhysicMaterial>().dynamicFriction = 0.1f;
                }
                else
                {
                    Debug.LogError("Fuck if I know what this is supposed to do");
                }

                break;

            case attribute.Size:
                //Change the Size of the Object
                if (pDiscriptor == discriptor.Increases)
                {
                    coll.transform.localScale *= 2;
                }
                else if (pDiscriptor == discriptor.Decreases)
                {
                    coll.transform.localScale /= 2;
                }
                else
                {
                    Debug.LogError("Fuck if I know what this is supposed to do");
                }
                break;

            case attribute.Weight:
                if(pDiscriptor == discriptor.Increases)
                {
                    coll.GetComponent<Rigidbody>().mass *= 2;
                }
                else if(pDiscriptor == discriptor.Decreases)
                {
                    coll.GetComponent<Rigidbody>().mass /= 2;
                }
                else
                {
                    Debug.LogError("Fuck if I know what this is supposed to do");
                }
                break;

            case attribute.Location:
                //Do something at Location It Hits(Cordinates)

                break;
        }

    }

}
