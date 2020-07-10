using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueEvent
{
    static Dictionary<string, GameObject> characters; // string name, GameObject reference to character
    static Dictionary<string, GameObject> uiElements; // string character name, GameObject ref to its UI chat bubble
    static DialogueText currentDialogue;
    public static bool inDialogue;
    static int lineNum;

    // for branching
    public static bool inBranch;
    public static int currOptionNum;
    
    public static void ExecuteEvent(DialogueText dialogueText)
    {
        currentDialogue = dialogueText;
        inDialogue = true;

        Debug.Log("Indexing characters in dialogue...");
        characters = new Dictionary<string, GameObject>(); // string name, GameObject reference to character
        foreach (List<SpeechLine> lines in currentDialogue.lines) // find characters speaking to get references to them
        {
            foreach(SpeechLine line in lines)
            {
                Debug.Log(line.speakerName);
                if(!characters.ContainsKey(line.speakerName)) // if this character hasn't been added yet
                {
                    // find and add the character object reference to the dictionary
                    characters.Add(line.speakerName, GameObject.Find(line.speakerName));
                }
            }
        }

        // apply dialogue effects
        ApplyDialogueEffects();

        // spawn text boxes above the entities for each character
        Debug.Log("Spawning text boxes...");
        uiElements = new Dictionary<string, GameObject>();
        foreach (KeyValuePair<string, GameObject> entry in characters)
        {
            if(!string.IsNullOrEmpty(entry.Key))
            {
                GameObject currTextBox = GameObject.Instantiate(DialogueEventController.dialogueBoxPrefab, DialogueEventController.dialogueCanvas.transform);

                currTextBox.GetComponentInChildren<DialogueBoxFollow>().characterToFollow = entry.Value;

                // set their nameplate
                foreach (Transform elem in currTextBox.GetComponentsInChildren<Transform>())
                {
                    if (elem.gameObject.tag == "Nameplate")
                    {
                        Debug.Log("Setting " + entry.Key + "'s nameplate...");
                        elem.gameObject.GetComponent<TMP_Text>().text = entry.Key;
                    }
                }

                currTextBox.SetActive(false); // hide when not in use
                uiElements.Add(entry.Key, currTextBox);
            }
        }
       
        // show first text
        lineNum = 0;
        ShowLine(currentDialogue.lines[lineNum][0]);

        if(CheckIfNextHasOptions(lineNum)) // check if next dialogue has options; if it is, show it at the same time (with synopsis)
        {
            inBranch = true;
            lineNum++;
            // show options
            ShowOptions(currentDialogue.lines[lineNum]);
        }
    }

    public static void ProgressDialogue()
    {
        if(lineNum + 1 < currentDialogue.lines.Count)
        {
            // progress dialogue if there are still lines
            lineNum++;

            if (inBranch)
            {
                if (CheckIfNextHasOptions(lineNum)) // check if next dialogue has options; if it is, show it at the same time (with synopsis)
                {
                    // if it's branching, fetch the option route
                    foreach(SpeechLine line in currentDialogue.lines[lineNum])
                    {
                        if(line.optionNum == currOptionNum)
                        {
                            // show this dialogue if it matches the option num chosen
                            ShowLine(line);
                        }
                    }
                }
                else
                {
                    inBranch = false;
                    currOptionNum = -1;
                    // show next dialogue
                    ShowLine(currentDialogue.lines[lineNum][0]);
                }
            }
        }
        else
        {
            // once you're out of lines, stop any effects
            RevertDialogueEffects();

            // clean up the UI elements
            foreach(GameObject elem in uiElements.Values)
            {
                GameObject.Destroy(elem);
            }

            // flush dialogue event vars
            inDialogue = false;
            lineNum = -1;
            currentDialogue = null;
            characters.Clear();
        }
    }

    static void ShowLine(SpeechLine line)
    {
        GameObject currDialogueUI = uiElements[line.speakerName];
        currDialogueUI.SetActive(true);

        DialogueBoxFollow dialogueBox = currDialogueUI.GetComponentInChildren<DialogueBoxFollow>();
        dialogueBox.currLine = line.lineText;

        // apply effects...
    }

    static void ShowOptions(List<SpeechLine> options)
    {
        // TO DO **********************8
    }

    static bool CheckIfNextHasOptions(int lineNum)
    {
        List<SpeechLine> nextLine = currentDialogue.lines[lineNum+1];
        if(nextLine.Count > 1)
        {
            return true;
        }
        return false;
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

    public static void ApplyDialogueEffects()
    {
        foreach (string str in currentDialogue.interactionEffects)
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

    public static void RevertDialogueEffects()
    {
        foreach (string str in currentDialogue.interactionEffects)
        {
            switch (str)
            {
                case "FREEZE_CHAR_ZOOM":
                //...TODO: GIVE BACK CHARACTER CONTROL*******************
                //break;
                case "FREEZE_CHAR": // TODO: MAKE THIS MORE GENERIC (CURRENTLY HARD-CODED FOR RU)
                                    

                    // delete the DOF controller...

                    break;
                default: break;
            }
        }
    }
    
}
