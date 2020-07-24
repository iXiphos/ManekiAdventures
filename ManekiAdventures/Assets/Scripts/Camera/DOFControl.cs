using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


//Taken from https://forum.unity.com/threads/how-can-i-access-post-processing-stack-volume-and-change-the-values-for-example-of-depth-of-field.537525/
// Modified for use in Maneki Adventure.
public class DOFControl : MonoBehaviour
{
    public float defaultFocus = 10f;
    public float defaultAperture = 5.6f;
    public float focusValue = 0.5f; // what the DOF changes to upon interaction
    public float apertureValue = 3.0f; // what the aperature changes to upon interaction
    public float rotationSpeed = 20f;
    public float playerHeight = 2f;

    public Vector3 additionalOffset = new Vector3(0, 0, 0);

    public PostProcessingProfile postProcProf;
    private DepthOfFieldModel.Settings defaultDOFVal;
    private DepthOfFieldModel.Settings dof;

    private DynamicCamera3D dc;
    private GameObject parentObj;
    private bool isFocusing;
    private bool hasExecutedFocus;

    // used in calculating camera pos and rotation
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Vector3 newCameraPosition;
    private Vector3 normalVector;
    private Vector3 midpoint;

    private void Start()
    {
        dc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DynamicCamera3D>(); //find dynamic camera
        parentObj = gameObject.GetComponentInParent<Transform>().gameObject; //find reference to parent
        dc.inInteraction = false;

        originalCameraRotation = dc.transform.rotation; // save the original position & rotation of the camera
        originalCameraPosition = dc.transform.position;

        // Depth of Field
        defaultDOFVal = postProcProf.depthOfField.settings; // save previous setting
        dof = defaultDOFVal; //set dof to default
        isFocusing = false;
        hasExecutedFocus = false;
    }

    public void ToggleFocusCamera()
    {
        if(dc != null)
        {
            // toggle, change later
            dc.inInteraction = !dc.inInteraction;
            isFocusing = !isFocusing;
            hasExecutedFocus = false;
            TurnAndZoomCamera();
        }
        else // if it is null, wait a moment then try calling this function again
        {
            StartCoroutine(WaitForInitThenRetryToggle());
        }
    }

    IEnumerator WaitForInitThenRetryToggle()
    {
        yield return new WaitForSeconds(0.05f);
        ToggleFocusCamera();
    }

    private void Update()
    {
        if(isFocusing)
        {
            TurnAndZoomCamera();
        }
        else
        {
            // reset DOF settings
            postProcProf.depthOfField.settings = defaultDOFVal;
           // dof.focusDistance = defaultFocus;
           // dof.aperture = defaultAperture;
           // postProcProf.depthOfField.settings = dof;
            ReturnToDefaultState();
        }
    }

    void TurnAndZoomCamera() // needs optimization so this isn't calculated every frame lol ***************
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Movement>().canMove = false;
        //player.GetComponent<Animator>().SetBool("isWaiting", true); // give it a waiting anim, idk

        if (!hasExecutedFocus)
        {
            // adjust depth of field & aperture
            dof.focusDistance = focusValue;
            dof.aperture = apertureValue;
            postProcProf.depthOfField.settings = dof;

            //originalCameraRotation = dc.transform.rotation; // save the original position & rotation of the camera
            originalCameraPosition = dc.transform.position;


            // find midpoint between player and interaction item
            float distanceBetweenPlayerAndObj = Vector3.Distance(player.transform.position, parentObj.transform.position);
            Vector3 ab = player.transform.position - parentObj.transform.position;
            midpoint = ((ab / ab.magnitude) * (distanceBetweenPlayerAndObj * 0.5f)) + parentObj.transform.position;
            float distanceBetweenCameraAndMidpoint = Vector3.Distance(originalCameraPosition, midpoint);

            //"zoom in" the camera (rotate & position)
            
            //Vector3 newCameraPositionY = Vector3.MoveTowards(originalCameraPosition, midpoint, distanceBetweenCameraAndMidpoint - distanceBetweenPlayerAndObj); // move forward (same distance away from midpoint as the distance between the two
            float cameraDisplacement = (distanceBetweenCameraAndMidpoint - distanceBetweenPlayerAndObj) / 2f;
            //newCameraPositionY += new Vector3(0, cameraDisplacement, 0); //calculating the Y
            

            Vector3 side1 = (parentObj.transform.position + new Vector3(0,1,0)) - parentObj.transform.position;
            Vector3 side2 = player.transform.position - parentObj.transform.position;

            // calculate which item is to the right (RELATIVE TO CAMERA) using dot product
            GameObject temp = new GameObject();
            temp.transform.position = parentObj.transform.position;
            temp.transform.LookAt(Camera.main.transform);
            temp.transform.Rotate(new Vector3(0, 1, 0), 180);
            float dot = Vector3.Dot(side2, temp.transform.right);
            if(dot >= 0) { 
                normalVector = Vector3.Cross(side1, side2); // if the player is to the right of the object, do perp (clockwise)
                //Debug.Log("Player is to the RIGHT");
            }
            else { 
                normalVector = -Vector3.Cross(side1, side2); // otherwise, do negative perp (counter-clockwise [negative perp]
                //Debug.Log("Player is to the LEFT");
            }
            GameObject.Destroy(temp);
            normalVector /= normalVector.magnitude; //normalize for a more predictable value

            //newCameraPosition = midpoint + (normalVector * cameraDisplacement) + new Vector3(0, (2 * newCameraPositionY.y / 3), 0);
            newCameraPosition = midpoint + (normalVector * cameraDisplacement) + new Vector3(0, 2, 0) + additionalOffset;

        }

        // lerp camera to calculated position & rotation
        dc.transform.position = Vector3.Lerp(dc.transform.position, newCameraPosition, Time.deltaTime * dc.lerpSpeed);
        dc.transform.rotation = Quaternion.RotateTowards(dc.transform.rotation, Quaternion.LookRotation((midpoint+new Vector3(0, playerHeight / 2, 0)) - dc.transform.position + additionalOffset), Time.deltaTime * dc.lerpSpeed * rotationSpeed);

        hasExecutedFocus = true; // don't repeat the calculations if they're already done
    }

    void ReturnToDefaultState()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canMove = true;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("isWaiting", false); // waiting anim

        /*
        if (!hasExecutedFocus)
        {
            // reset DOF settings
            //postProcProf.depthOfField.settings = defaultDOFVal;
            dof.focusDistance = defaultFocus;
            dof.aperture = defaultAperture;
            postProcProf.depthOfField.settings = dof;
            Debug.Log("Returning to default");
        }*/

        // put camera rotation back to its original state before interaction
        if (!(dc.transform.rotation.eulerAngles == originalCameraRotation.eulerAngles)) {
            dc.transform.rotation = Quaternion.RotateTowards(dc.transform.rotation, originalCameraRotation, Time.deltaTime * dc.lerpSpeed * rotationSpeed * 2f);
        }
        
        // DynamicCamera3D will put the camera back to its correct position
        
        hasExecutedFocus = true;
    }

    private void OnDestroy()
    {
        //postProcProf.depthOfField.settings = defaultDOFVal;
        dof.focusDistance = defaultFocus;
        dof.aperture = defaultAperture;
        postProcProf.depthOfField.settings = dof;
    }
}
