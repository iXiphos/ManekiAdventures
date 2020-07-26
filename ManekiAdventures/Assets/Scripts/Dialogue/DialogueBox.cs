using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    protected Camera mainCamera;
    protected TMP_Text textUI;
    public string currLine;
    public Vector3 uiDisplacement;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("DynamicCamera3D").GetComponent<Camera>();
        foreach (TMP_Text obj in gameObject.GetComponentsInChildren<TMP_Text>())
        {
            if (obj.tag == "DialogueText") { textUI = obj; }
        }
    }

    void FixedUpdate()
    {
        if (textUI != null)
        {
            // assign text
            textUI.text = currLine;
        }
    }

}
