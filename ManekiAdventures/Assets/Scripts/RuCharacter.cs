using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuCharacter : MonoBehaviour
{
    DialogueEventController dialogueEventController;
    public float interactDistance = 4f;

    // Start is called before the first frame update
    void Start()
    {
        dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();
    }

    // Update is called once per frame
    void Update()
    {
        // SAMPLE
        if (!DialogueEvent.inDialogue)
        {
            //Debug.Log(Vector3.Distance(gameObject.transform.position, GameObject.Find("KIKI").transform.position));
            if (Input.GetKeyUp(KeyCode.Space) && (Vector3.Distance(gameObject.transform.position, GameObject.Find("KIKI").transform.position) <= interactDistance))
            {
                Dictionary<string, string> vars = new Dictionary<string, string>();
                vars.Add("item", "piece of moonstone");
                dialogueEventController.ExecuteEventWithVars("SAMPLE_DIALOGUE", vars);
            }
        }
    }
}
