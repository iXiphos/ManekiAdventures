using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tossPotion : MonoBehaviour
{

    [SerializeField]
    public GameObject potion;

    public GameObject potionTemplete;

    public LayerMask clickMask;

    public Item potionItem;

    bool active;

    public float speed;

    public GameObject hand;

    public float h = 25;
    public float gravity = -18;

    public bool debugPath;
    Vector3 clickPosition;
    GameObject target;

    Color output;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 30;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator createTarget()
    {
        lineRenderer.enabled = true;
        Ray ray;
        RaycastHit hit;
        clickPosition = -Vector3.one;
        active = true;
        potion.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        while (active)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 100f, clickMask))
            {
                clickPosition = hit.point;
            }
            if(hit.collider.tag == "Interactable" || hit.collider.tag == "AltInteractable")
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }
            else
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Inventory.instance.Add(potionItem);
                Destroy(potion);
                potionItem = null;
                potion = null;
                break;
            }
            if (debugPath)
            {
                DrawPath();
            }
            if (Input.GetMouseButtonDown(0) && (hit.collider.tag == "Interactable" || hit.collider.tag == "AltInteractable"))
            {
                Debug.LogError(clickPosition);
                potion.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                // Throw animation & turn player
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Animator>().SetTrigger("throw");
                player.transform.LookAt(new Vector3(hit.point.x, player.transform.position.y, hit.point.z));

                // play throw sound
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("whoosh");

                potion.transform.parent = null;
                Launch();
                lineRenderer.enabled = false;
                potion = null;
                active = false;
                break;
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

    void Launch() {
		Physics.gravity = Vector3.up * gravity;
		potion.GetComponent<Rigidbody>().useGravity = true;
		potion.GetComponent<Rigidbody>().velocity = CalculateLaunchData ().initialVelocity;
	}

	LaunchData CalculateLaunchData() {
		float displacementY = clickPosition.y - potion.GetComponent<Rigidbody>().position.y;
		Vector3 displacementXZ = new Vector3 (clickPosition.x - potion.GetComponent<Rigidbody>().position.x, 0, clickPosition.z - potion.GetComponent<Rigidbody>().position.z);
		float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
	}

	void DrawPath() {
		LaunchData launchData = CalculateLaunchData ();
        lineRenderer = GetComponent<LineRenderer>();

        int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up *gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = potion.GetComponent<Rigidbody>().position + displacement;
            lineRenderer.SetPosition(i - 1, drawPoint);
		}
	}

	struct LaunchData {
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData (Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
		
	}

}