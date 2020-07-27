using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tossPotion : MonoBehaviour
{

    [SerializeField]
    public GameObject potion;

    public GameObject potionTemplete;

    public LayerMask clickMask;

    bool active;

    public float speed;

    public float smooth;

    public GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator createTarget()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 clickPosition = -Vector3.one;
        active = true;
        while (active)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, clickMask))
            {
                clickPosition = hit.point;
                if(hit.collider.gameObject.tag == "Interactable")
                {

                }
            }

            if (Input.GetMouseButtonDown(0) && (hit.collider.tag == "Interactable" || hit.collider.tag == "AltInteractable"))
            {
                // I sure do hope this is the right place to put this.
                GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>().ExecuteEvent("UNIQUE_THROWPOTION");
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("throw");

                potion.transform.parent = null;
                while (true)
                {
                    Vector3 dir = clickPosition - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    float step = speed * Time.deltaTime;
                    
                    potion.transform.position = Vector3.MoveTowards(potion.transform.position, clickPosition, step);
                    yield return null;
                    if (Vector3.Distance(dir, potion.transform.position) < 0.2)
                    {
                        active = false;
                        break;
                    }
                }
            }
            yield return null;
        }
    }

    public void createPotion(Item item)
    {
        GameObject temp = Instantiate(potionTemplete, hand.transform.position, hand.transform.rotation);
        temp.transform.parent = hand.transform;
        temp.GetComponent<Potion>().pAttribute = item.Attribute;
        temp.GetComponent<Potion>().pDiscriptor = item.Discriptor;
        //temp.SetActive(false);
        potion = temp;
        Debug.Log("Setting potion.");
    }

}