using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventController : MonoBehaviour
{
    public GameObject dialogueCanvasRef;
    public GameObject dialogueBoxPrefabRef;
    public GameObject dofControllerRef;
    public static GameObject dialogueCanvas;
    public static GameObject dialogueBoxPrefab;
    public static GameObject dofController;

    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas = dialogueCanvasRef;
        dialogueBoxPrefab = dialogueBoxPrefabRef;
        dofController = dofControllerRef;

        
        // DEBUG:
        ExecuteEvent("SAMPLE_DIALOGUE");
    }

    // Update is called once per frame
    void Update()
    {
        if(!DialogueEvent.inBranch)
        {
            if (Input.GetKeyDown(KeyCode.Space) && DialogueEvent.inDialogue)
            {
                DialogueEvent.ProgressDialogue();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q) && DialogueEvent.inDialogue)
            {
                DialogueEvent.currOptionNum = 1;
                DialogueEvent.ProgressDialogue();
            }
            else if (Input.GetKeyDown(KeyCode.E) && DialogueEvent.inDialogue)
            {
                DialogueEvent.currOptionNum = 2;
                DialogueEvent.ProgressDialogue();
            }
        }
    }

    public void ExecuteEvent(string filename)
    {
        Debug.Log("Executing dialogue event: " + filename);

        // fetch the dialogue information
        DialogueText dialogueText = new DialogueText();
        dialogueText.ReadRawLinesFromFile(filename);
        DialogueEvent.ExecuteEvent(dialogueText);
    }

    // NOTE: SPEECHLINE VARIABLES ARE {{ }}, THIS CLASS SHOULD INJECT THE CORRECT VARIABLE FOR THE SITUATION
    public void ExecuteEventWithVars(string filename, Dictionary<string, string> vars) // vars should be: variable name, variable value
    {
        // fetch the dialogue information
        DialogueText dialogueText = new DialogueText();
        dialogueText.ReadRawLinesFromFile(filename);

        // inject {{vars}} in order 
        foreach(KeyValuePair<string, string> entry in vars)
        {
            // search each line for the variable and inject as needed
            foreach (List<SpeechLine> lines in dialogueText.lines)
            {
                foreach (SpeechLine line in lines)
                {
                    if(line.lineText.Contains("{{" + entry.Key + "}}"))
                    {
                        Debug.Log("Injecting var " + entry.Key + " into " + filename + "...");
                        line.lineText = line.lineText.Replace("{{" + entry.Key + "}}", entry.Value);
                    }
                }
            }
        }

        // execute event
        DialogueEvent.ExecuteEvent(dialogueText);
    }
}
