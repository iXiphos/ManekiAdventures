using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxFollow : MonoBehaviour
{
    Camera mainCamera;
    public Vector3 uiDisplacement;
    TMP_Text textUI;
    public GameObject characterToFollow;
    public string currLine;

    public Vector3 displacement = new Vector3(0, 0, 0);
    //float ruHeight = 4; // TO DO: implement if needed
    //float kikiHeight = 5;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("DynamicCamera3D").GetComponent<Camera>();
        foreach (TMP_Text obj in gameObject.GetComponentsInChildren<TMP_Text>())
        {
            if(obj.tag == "DialogueText") { textUI = obj; }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // follow character
        if (characterToFollow != null)
        {
            Vector3 uiPos = mainCamera.WorldToScreenPoint(characterToFollow.transform.position + displacement);
            gameObject.transform.position = uiPos + uiDisplacement;

        }

        if (textUI != null)
        {
            // assign text
            textUI.text = currLine;
        }
    }

    private void Update()
    {
        // NOTE THAT THIS IS HARDCODED AND SHOULD BE FIXED!!!!!!!!!!1 ********************
        //GameObject ru = GameObject.Find("RU");
        //GameObject kiki = GameObject.Find("KIKI");
        //displacement = new Vector3(0, Vector3.Distance(ru.transform.position, kiki.transform.position), 0);
        displacement = new Vector3(0, 4.5f, 0);
    }
}
