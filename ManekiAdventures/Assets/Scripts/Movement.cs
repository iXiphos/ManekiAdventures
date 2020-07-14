using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool canMove;
    Vector3 inputMovement;
    public float moveSpeed;
    Vector3 pos, velocity;
    //public float animspeed = 1.2f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        canMove = true;
        pos = transform.position;
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveHorizontal();
        AnimateWalking();
        AnimatePickup();
    }

    void MoveHorizontal()
    {
        if(canMove)
        {
            // translate
            inputMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.Translate(inputMovement * Time.deltaTime * moveSpeed, Space.World);

            // rotate to look the appropriate direction
            if(inputMovement.x != 0 || inputMovement.z != 0)
                transform.eulerAngles = Vector3.up * ((Mathf.Atan2(inputMovement.x, inputMovement.z) * Mathf.Rad2Deg) + 45f);
        }
    }

    void AnimateWalking()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    void AnimatePickup()
    {
        if (Input.GetKeyUp(KeyCode.Q)) // DEBUG: LINK THIS UP TO ACTUAL INTERACT.
        {
            StartCoroutine(PickupAnimation());
        }
    }

    IEnumerator PickupAnimation()
    {
        animator.SetTrigger("triggerPickup");
        canMove = false;
        yield return new WaitForSeconds(1.1f);
        canMove = true;
    }
}
