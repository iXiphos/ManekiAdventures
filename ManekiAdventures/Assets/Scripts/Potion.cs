using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public attribute pAttribute;

    public discriptor pDiscriptor;

    public int explosionSize;

    private Rigidbody rb;

    public float WeightIncrease;
    public float WeightDecrease;

    public float SizeIncrease;
    public float SizeDecrease;

    public GameObject explosion;

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
        if(collision.tag == "Interactable" || collision.tag == "SoccerBall")
        {
            StartCoroutine(Explosion(collision.gameObject));
            if(collision.gameObject.GetComponent<UniquePotionInteraction>() != null)
            {
                collision.gameObject.GetComponent<UniquePotionInteraction>().ExecuteUniqueInteraction();
            }
            // play sound
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("breaking-glass-bottle");
            // play anim
            GameObject explosionGame = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(explosionGame, 2.0f);
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
                    coll.gameObject.transform.localScale *= SizeIncrease;
                    Debug.Log("Grow");
                    Destroy(gameObject);
                }
                else if (pDiscriptor == discriptor.Decreases)
                {
                    coll.gameObject.transform.localScale /= SizeDecrease;
                    Debug.Log("Shrink");
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
                    coll.GetComponent<Rigidbody>().mass *= WeightIncrease;
                    Destroy(gameObject);
                }
                else if(pDiscriptor == discriptor.Decreases)
                {
                    coll.GetComponent<Rigidbody>().mass /= WeightDecrease;
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("Fuck if I know what this is supposed to do");
                }
                break;

            case attribute.Location:
                //Do something at Location It Hits(Cordinates)**
                if (pDiscriptor == discriptor.Frozen)
                {
                    coll.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
                    Destroy(gameObject);
                }
                break;
        }

    }

}
