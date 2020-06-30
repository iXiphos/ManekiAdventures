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
        if (!active && potion != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                potion.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
            }
        }
    }

    IEnumerator createTarget()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 clickPosition = -Vector3.one;
        while (active)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, clickMask))
                clickPosition = hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                while (true)
                {
                    Vector3 dir = clickPosition - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(dir);
                    potion = Instantiate(potion, transform);
                    potion.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smooth);
                    transform.Translate(transform.forward * speed);
                }
            }
            yield return null;
        }
    }

    public void createPotion(Item item)
    {
        GameObject temp = Instantiate(potionTemplete, hand.transform.position, hand.transform.rotation);
        temp.GetComponent<Potion>().pAttribute = item.Attribute;
        temp.GetComponent<Potion>().pDiscriptor = item.Discriptor;
        //temp.SetActive(false);
        potion = temp;
    }

}
