using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxFollow : MonoBehaviour
{
    Camera mainCamera;
    TMP_Text textUI;
    public GameObject characterToFollow;
    public string currLine;

    Vector3 displacement = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("DynamicCamera3D").GetComponent<Camera>();
        textUI = gameObject.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // follow character
        if(characterToFollow != null)
        {
            gameObject.transform.position = mainCamera.WorldToScreenPoint(characterToFollow.transform.position + displacement);
        }

        if (textUI != null)
        {
            // assign text
            textUI.text = currLine;
        }
    }
}
