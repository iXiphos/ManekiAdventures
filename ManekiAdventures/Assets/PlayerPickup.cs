using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public int radius;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // play pickup animation
            gameObject.GetComponent<Movement>().AnimatePickup();

            // pickup item
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            if (hitColliders.Length != 0)
            {
                for(int i = 0; i < hitColliders.Length; i++)
                {
                    if(hitColliders[i] != null)
                        if(hitColliders[i].tag == "Component")
                        {
                            Debug.LogError(hitColliders[i].GetComponent<ItemPickup>().item);
                            GameObject.FindGameObjectWithTag("GameController").GetComponent<StoryEventHandler>().PickedUpEvent(hitColliders[i].GetComponent<ItemPickup>().item.name); // execute dialogue as needed
                            Inventory.instance.Add(hitColliders[i].GetComponent<ItemPickup>().item);
                            Destroy(hitColliders[i].gameObject);
                            break;
                        }
                }
            }
        }
    }
}
