using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueEvent
{
    static Dictionary<string, GameObject> characters; // string name, GameObject reference to character
    static Dictionary<string, GameObject> uiElements; // string character name, GameObject ref to its UI chat bubble
    public static DialogueText currentDialogue;
    public static bool inDialogue;
    public static int lineNum;

    // for branching
    public static bool inBranch;
    public static bool isChoosing = false;
    public static int currOptionNum;

    // for effects
    static GameObject dofControl;
    
    public static void ExecuteEvent(DialogueText dialogueText)
    {
        currentDialogue = dialogueText;
        inDialogue = true;

        //Debug.Log("Indexing characters in dialogue...");
        characters = new Dictionary<string, GameObject>(); // string name, GameObject reference to character
        foreach (List<SpeechLine> lines in currentDialogue.lines) // find characters speaking to get references to them
        {
            foreach(SpeechLine line in lines)
            {
                //Debug.Log(line.speakerName);
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

                currTextBox.GetComponent<DialogueBoxFollow>().characterToFollow = entry.Value;

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
            Debug.Log("Showing options...");
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

            if (CheckIfNextHasOptions(lineNum)) // check if next dialogue has options; if it is, show it at the same time (with synopsis)
            {
                // detect if next line is kiki
                string nextSpeaker = "";
                foreach (SpeechLine line in currentDialogue.lines[lineNum+1])
                {
                    if (!string.IsNullOrEmpty(line.speakerName))
                    {
                        nextSpeaker = line.speakerName;
                        break;
                    }
                }

                // if it's branching, fetch the option route
                if(nextSpeaker == "KIKI")
                {
                    inBranch = true;
                    isChoosing = true;
                    ShowLine(currentDialogue.lines[lineNum][0]);
                    ShowOptions(currentDialogue.lines[lineNum+1]);
                }
                else 
                {
                    foreach (SpeechLine line in currentDialogue.lines[lineNum])
                    {
                        if (line.optionNum == currOptionNum)
                        {
                            // show this dialogue if it matches the option num chosen
                            isChoosing = false;
                            ShowLine(line);
                            continue;
                        }
                    }  
                }
            }
            else if(CheckIfThisHasOptions(lineNum))
            {
                foreach (SpeechLine line in currentDialogue.lines[lineNum])
                {
                    if (line.optionNum == currOptionNum)
                    {
                        // show this dialogue if it matches the option num chosen
                        isChoosing = false;
                        ShowLine(line);
                        continue;
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
        else
        {
            // once you're out of lines, stop any effects
            Debug.Log("Completing dialogue. Cleaning up...");
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
        // hide other ui boxes
        foreach(GameObject obj in uiElements.Values)
        {
            obj.SetActive(false);
        }
        
        GameObject currDialogueUI = uiElements[line.speakerName];
        DialogueBoxFollow dialogueBox = currDialogueUI.GetComponent<DialogueBoxFollow>();
        dialogueBox.currLine = line.lineText;

        // show this ui box
        currDialogueUI.SetActive(true);

        // apply effects...
    }

    static void ShowOptions(List<SpeechLine> options)
    {
        Debug.Log("Showing options...");
        string speaker = "";
        foreach(SpeechLine line in options)
        {
            if(!string.IsNullOrEmpty(line.speakerName))
            {
                speaker = line.speakerName;
                break;
            }
        }

        GameObject currDialogueUI = uiElements[speaker];
        DialogueBoxFollow dialogueBox = currDialogueUI.GetComponent<DialogueBoxFollow>();
        
        // put together a dialogue option line
        string optionsLineText = "";
        foreach(SpeechLine line in options)
        {
            optionsLineText += line.optionNum + ": " + line.synopsisText;
            optionsLineText += "\n";
        }

        dialogueBox.currLine = optionsLineText;

        // show this ui box
        currDialogueUI.SetActive(true);
    }

    static bool CheckIfNextHasOptions(int lineNum)
    {
        return CheckIfThisHasOptions(lineNum+1);
    }

    static bool CheckIfThisHasOptions(int lineNum)
    {
        if (lineNum < currentDialogue.lines.Count)
        {
            List<SpeechLine> nextLine = currentDialogue.lines[lineNum];
            if (nextLine.Count > 1)
            {
                return true;
            }
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
                    if(characters["RU"].GetComponentInChildren<DOFControl>() == null)
                    {
                        dofControl = GameObject.Instantiate(DialogueEventController.dofController, characters["RU"].transform);
                    }
                    else
                    {
                        dofControl = characters["RU"].GetComponentInChildren<DOFControl>().gameObject;
                    }
                    dofControl.GetComponent<DOFControl>().ToggleFocusCamera();

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
                    if(dofControl != null)
                        dofControl.GetComponent<DOFControl>().ToggleFocusCamera();
                    
                    // delete the DOF controller...

                    break;
                default: break;
            }
        }
    }
    
}
