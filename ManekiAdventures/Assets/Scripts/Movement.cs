using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool canMove;
    Vector3 inputMovement;
    public float moveSpeed;
    Vector3 pos, velocity;
    //float heightOffset = 1.8f;
    float rayDisplacement = 0.5f;
    public float fakeGravityIntensity = 5f;

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
            float rad45 = -45f * (Mathf.PI / 180f);
            Vector3 isoRotate = new Vector3(inputMovement.x * Mathf.Cos(rad45) - inputMovement.z * Mathf.Sin(rad45), inputMovement.y, inputMovement.x * Mathf.Sin(rad45) + inputMovement.z * Mathf.Cos(rad45));


            transform.Translate(isoRotate * Time.deltaTime * moveSpeed, Space.World);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, CalculateYValueOfTerrain(), transform.position.z), Time.deltaTime * fakeGravityIntensity); // adjust y position

            // rotate to look the appropriate direction
            if (inputMovement.x != 0 || inputMovement.z != 0)
                transform.eulerAngles = Vector3.up * ((Mathf.Atan2(inputMovement.x, inputMovement.z) * Mathf.Rad2Deg) + 45f);
        }
    }

    float CalculateYValueOfTerrain()
    {
        // cast a ray slightly in front of the player
        
        float yVal = 0f;

        RaycastHit hit;
        Ray ray = new Ray(gameObject.transform.position + new Vector3(0,2,0) + (gameObject.transform.forward * rayDisplacement), Vector3.down);
       // Debug.DrawLine(gameObject.transform.position + new Vector3(0, 2, 0) + (gameObject.transform.forward * rayDisplacement), gameObject.transform.position + (gameObject.transform.forward * rayDisplacement) + new Vector3(0, -10, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.transform.tag == "Terrain")
            {
                Debug.Log("hit terrain. Y is " + hit.point.y);
                yVal = hit.point.y;
            }
        }
        return yVal;
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
