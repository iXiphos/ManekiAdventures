using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


//Taken from https://forum.unity.com/threads/how-can-i-access-post-processing-stack-volume-and-change-the-values-for-example-of-depth-of-field.537525/
// Modified for use in Maneki Adventure.
public class DOFControl : MonoBehaviour
{
    public float focusValue = 0.5f; // what the DOF changes to upon interaction
    public PostProcessingProfile postProcProf;
    private DepthOfFieldModel.Settings defaultDOFVal;
    private DepthOfFieldModel.Settings dof;

    private DynamicCamera3D dc;
    private GameObject parentObj;
    private bool isFocusing;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private void Start()
    {
        dc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DynamicCamera3D>(); //find dynamic camera
        parentObj = gameObject.GetComponentInParent<Transform>().gameObject; //find reference to parent

        // Depth of Field
        defaultDOFVal = postProcProf.depthOfField.settings; // save previous setting
        dof = defaultDOFVal;
        isFocusing = false;
    }

    private void Update()
    {
        // toggle, change later
        if (Input.GetKeyDown(KeyCode.Space)) // ***** THIS SHOULD BE REPLACED WITH THE INTERACTION TRIGGER ********
        {                                   // THE PARENT SHOULD NOTIFY ANY CHILDREN CONTROLS OF AN INTERACTION (& prevent the character from moving) ***********
            dc.inInteraction = !dc.inInteraction;
            isFocusing = !isFocusing;
            TurnAndZoomCamera();
        }

        if(isFocusing)
        {
            TurnAndZoomCamera();
        }
        else
        {
            ReturnToDefaultState();
        }
    }

    void TurnAndZoomCamera() // needs optimization so this isn't calculated every frame lol ***************
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // find midpoint between player and interaction item
        Vector3 midpoint = (player.transform.position + parentObj.transform.position) * 0.5f;
        float distanceBetweenPlayerAndObj = Vector3.Distance(player.transform.position, parentObj.transform.position);
        float distanceBetweenCameraAndMidpoint = Vector3.Distance(dc.transform.position, midpoint);

        //"zoom in" the camera (rotate & position)
        originalCameraRotation = dc.transform.rotation; // save the original position & rotation of the camera
        originalCameraPosition = dc.transform.position;

        
        Vector3 newCameraPosition = Vector3.MoveTowards(originalCameraPosition, midpoint, distanceBetweenCameraAndMidpoint - distanceBetweenPlayerAndObj); // move forward (same distance away from midpoint as the distance between the two
        newCameraPosition += new Vector3(0, -(distanceBetweenCameraAndMidpoint - distanceBetweenPlayerAndObj)/2, 0);
        dc.transform.position = Vector3.Lerp(originalCameraPosition, newCameraPosition, Time.deltaTime * dc.lerpSpeed);

        //if (dc.transform.position == newCameraPosition)
        Quaternion.Slerp(dc.gameObject.transform.rotation, Quaternion.LookRotation(midpoint - dc.transform.position), Time.deltaTime);
        
        //Quaternion.Slerp(dc.transform.rotation, dc.gameObject.transform.LookAt(midpoint), Time.deltaTime);
            //dc.gameObject.transform.LookAt(midpoint); // rotate to look at the midpoint

        // adjust depth of field
        dof.aperture = focusValue;
        postProcProf.depthOfField.settings = dof;
    }

    void ReturnToDefaultState()
    {
        postProcProf.depthOfField.settings = defaultDOFVal;

        // zoom out the camera

        // put camera rotation back to its original state before interaction
    }

    private void OnDestroy()
    {
        ReturnToDefaultState();
    }
}
