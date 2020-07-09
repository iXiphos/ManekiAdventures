using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent
{

    // NOTE: SPEECHLINE VARIABLES ARE {{ }}, THIS CLASS SHOULD INJECT THE CORRECT VARIABLE FOR THE SITUATION

    public static void ExecuteEvent(string filename)
    {
        // fetch the dialogue information
        DialogueText dialogueText = new DialogueText();
        dialogueText.ReadRawLinesFromFile(filename);

        // find characters speaking to get references to them
        // FOR THIS BUILD: just ru and maneki will ever speak
        // ....
        // string name, GameObject reference to character
        Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();
        characters.Add("RU", GameObject.Find("RU"));
        characters.Add("KIKI", GameObject.Find("KIKI"));

        // apply dialogue effects
        ApplyDialogueEffects(dialogueText, characters);

        // spawn text boxes above the entities
        GameObject ruWorldspaceUI = GameObject.Instantiate(DialogueEventController.worldspaceUIPrefab, characters["RU"].transform);
        GameObject kikiWorldspaceUI = GameObject.Instantiate(DialogueEventController.worldspaceUIPrefab, characters["KIKI"].transform);

        // hide/show/assign text as needed
        for(int i = 0; i < dialogueText.lines.Count; i++)
        {
            List<SpeechLine> currLine = dialogueText.lines[i];
            // check if next dialogue has options; if it is, show it at the same time (with synopsis)
        }

        foreach (List<SpeechLine> lines in dialogueText.lines)
        {
            if (lines.Count > 1)
            {
                // do branching...
            }
            else // singular text line for 1 person
            {

            }
            /*foreach (SpeechLine line in lines)
            {
                Debug.Log(line.lineText);
            }*/
        }

    }

    void ExecuteEvent(string filename, string[] vars) // if the lines have vars, pass them in with a string array
    {

    }

    public static void DebugPrintEvent()
    {
        // DEBUG TESTING:
        DialogueText testText = new DialogueText();
        testText.ReadRawLinesFromFile("SAMPLE_DIALOGUE");

        string effects = "";
        foreach (string str in testText.interactionEffects)
        {
            effects += str + " ";
        }
        Debug.Log(effects);

        Debug.Log("Number of Lines: " + testText.lines.Count);
        foreach(List<SpeechLine> lines in testText.lines)
        {
            foreach(SpeechLine line in lines)
            {
                Debug.Log(line.lineText);
            }
        }
       
    }

    public static void ApplyDialogueEffects(DialogueText dt, Dictionary<string, GameObject> characters)
    {
        foreach (string str in dt.interactionEffects)
        {
            switch(str)
            {
                case "FREEZE_CHAR_ZOOM":
                    //...TODO: FREEZE CHARACTER CONTROL*******************
                    //break;
                case "FREEZE_CHAR": // TODO: MAKE THIS MORE GENERIC (CURRENTLY HARD-CODED FOR RU)
                    GameObject.Instantiate(DialogueEventController.dofController, characters["RU"].transform);
                    break;
                default: break;
            }
        }
    }
    
}
