using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public attribute pAttribute;

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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Interactable")
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
                    Destroy(gameObject);
                }
                else if (pDiscriptor == discriptor.Decreases)
                {
                    coll.GetComponent<PhysicMaterial>().dynamicFriction = 0.1f;
                    Destroy(gameObject);
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
                    coll.gameObject.transform.localScale *= 2;
                    Debug.Log("Grow");
                    Destroy(gameObject);
                }
                else if (pDiscriptor == discriptor.Decreases)
                {
                    coll.gameObject.transform.localScale /= 2;
                    Destroy(gameObject);
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
                    Destroy(gameObject);
                }
                else if(pDiscriptor == discriptor.Decreases)
                {
                    coll.GetComponent<Rigidbody>().mass /= 2;
                    Destroy(gameObject);
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
