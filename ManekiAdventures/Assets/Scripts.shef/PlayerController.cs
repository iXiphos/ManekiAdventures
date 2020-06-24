using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMoter))]
public class PlayerController : MonoBehaviour
{
    Camera cam; //setting camera variable
    PlayerMoter moter;

    public Interactable focus;  //makes the interactable object get focused on by the camera

    public LayerMask movementMask;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; //setting the camera
        moter = GetComponent<PlayerMoter>();
    }

    // Update is called once per frame
    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

  

        if (Input.GetMouseButtonDown(0)) //using left mouse click will move the player to that poing
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                moter.MoveToPoint(hit.point);

                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(1)) //using right mouse click will interact with an object
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus (Interactable newFocus) //sets the camera focus to not be the player
    {

        if(newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;
            moter.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);

    }

    void RemoveFocus() //stops focusing on object, resets to player
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        moter.StopFollowingTarget();
    }
}
