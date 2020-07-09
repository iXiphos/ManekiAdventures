using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventController : MonoBehaviour
{
    public static GameObject worldspaceUIPrefab;
    public static GameObject dofController;

    // Start is called before the first frame update
    void Start()
    {
        DialogueEvent.DebugPrintEvent();
       // DialogueEvent.ExecuteEvent("SAMPLE_DIALOGUE");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
