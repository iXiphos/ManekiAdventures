using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueEvent : MonoBehaviour
{
    static Dictionary<string, GameObject> characters; // string name, GameObject reference to character
    static Dictionary<string, GameObject> uiElements; // string character name, GameObject ref to its UI chat bubble
    public static DialogueText currentDialogue;
    public static bool inDialogue;
    public static bool inStaticDialogue;
    public static int lineNum;
    static SpeechLine currLine;
    public static bool inLine;

    // for branching
    public static bool inBranch;
    public static bool isChoosing = false;
    public static int currOptionNum;

    // for effects
    static GameObject dofControl;
    
    public static void ExecuteEvent(DialogueText dialogueText)
    {
        ClearPreviousEvent();

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
        //Debug.Log("Spawning text boxes...");
        uiElements = new Dictionary<string, GameObject>();
        foreach (KeyValuePair<string, GameObject> entry in characters)
        {
            if(!string.IsNullOrEmpty(entry.Key))
            {
                GameObject currTextBox;

                if (dialogueText.interactionEffects.Contains("STATIC"))
                {
                    inStaticDialogue = true;
                    GameObject.Find("UICanvases").GetComponent<CanvasManager>().SetGamestateByCanvasName("DialogueCanvas");
                    currTextBox = GameObject.Instantiate(DialogueEventController.dialogueBoxPrefab, DialogueEventController.dialogueCanvas.transform);
                }
                else // default to moving box
                {
                    inStaticDialogue = false;
                    currTextBox = GameObject.Instantiate(DialogueEventController.dialogueBoxFollowPrefab, DialogueEventController.dialogueCanvas.transform);
                    currTextBox.GetComponent<DialogueBoxFollow>().characterToFollow = entry.Value;
                }
                
                currTextBox.SetActive(false); // hide when not in use

                // set their nameplate
                foreach (Transform elem in currTextBox.GetComponentsInChildren<Transform>())
                {
                    if (elem.gameObject.tag == "Nameplate")
                    {
                        //Debug.Log("Setting " + entry.Key + "'s nameplate...");
                        elem.gameObject.GetComponent<TMP_Text>().text = entry.Key;
                    }
                }

                uiElements.Add(entry.Key, currTextBox);
            }
        }
       
        // show first text
        lineNum = 0;
        ShowLine(currentDialogue.lines[lineNum][0]);

        if(CheckIfNextHasOptions(lineNum)) // check if next dialogue has options; if it is, show it at the same time (with synopsis)
        {
            //Debug.Log("Showing options...");
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
            //Debug.Log("Completing dialogue. Cleaning up...");
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
        inLine = true;
        currLine = line;
        // hide other ui boxes
        foreach (GameObject obj in uiElements.Values)
        {
            obj.SetActive(false);
        }
        
        GameObject currDialogueUI = uiElements[line.speakerName];
        DialogueBox dialogueBox = currDialogueUI.GetComponent<DialogueBox>();
        //dialogueBox.currLine = line.lineText;

        // show this ui box
        currDialogueUI.SetActive(true);

        // apply effects...
        float typingSpeed = 0.03f; // default speed
        switch(line.lineEffect)
        {
            case LineEffect.SHAKE: // ********** TO DO *******************
                dialogueBox.StartCoroutine(ShakeUIItem(dialogueBox));
                break;
            case LineEffect.SLOW:
                typingSpeed = 0.2f;
                break;
            case LineEffect.FAST:
                typingSpeed = 0.0001f;
                break;
            default: break;
        }

        //Debug.Log("Type Speed: " + typingSpeed);

        // "type out" the dialogue
        dialogueBox.StartCoroutine(TypeDialogue(dialogueBox, line.lineText, typingSpeed));
    }

    static public void ShowFullLine()
    {
        inLine = false;
        GameObject currDialogueUI = uiElements[currLine.speakerName];
        currDialogueUI.GetComponent<DialogueBox>().currLine = currLine.lineText;
        
    }

    private static IEnumerator TypeDialogue(DialogueBox dialogueBox, string lineToType, float typingSpeed)
    {
        dialogueBox.currLine = ""; // flush text

        // get raw text
        List<string> typingText = new List<string>();
        string phraseToAdd = "";
        bool keepAdding = false;
        foreach(char letter in lineToType) // collect any formatting areas (they should be injected immediately so they don't get typed out)
        {
            if(letter == '<')
            {
                phraseToAdd = "";
                keepAdding = true;
                phraseToAdd += letter;
            }
            else if(keepAdding && letter == '>')
            {
                keepAdding = false;
                phraseToAdd += letter;
                typingText.Add(phraseToAdd);
                phraseToAdd = "";
            }
            else if(keepAdding)
                phraseToAdd += letter;
            else
                typingText.Add(letter.ToString());
        }
        

        // populate with string (chars) and inject formatting immediately
        foreach (string chunk in typingText)
        {
            if(inLine)
            {
                dialogueBox.currLine += chunk;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }

        dialogueBox.currLine = lineToType;
        inLine = false;
    }

    private static IEnumerator ShakeUIItem(DialogueBox ui)
    {
        //bool isShaking = true;
        float duration = 0.5f;
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float shakeX = Mathf.Pow(duration/70f, duration) * Mathf.Sin(timeElapsed * 70f) * 500f;
            float shakeY = Mathf.Pow(duration / 70f, duration) * Mathf.Sin(timeElapsed * 50f) * 500f;
            ui.uiDisplacement = Vector3.Lerp(ui.uiDisplacement, new Vector3(shakeX, shakeY, 0), Time.deltaTime*5f);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        
        yield return new WaitForSecondsRealtime(duration);
        timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            Vector3.Lerp(ui.uiDisplacement, new Vector3(0, 0, 0), Time.deltaTime * 5f);
        }
        
    }

    static void ShowOptions(List<SpeechLine> options)
    {
        isChoosing = true;
        GameObject currDialogueUI = uiElements[currLine.speakerName];
        DialogueBox dialogueBox = currDialogueUI.GetComponent<DialogueBox>();
        dialogueBox.StartCoroutine(TryShowOptions(options));
    }

    static IEnumerator TryShowOptions(List<SpeechLine> options)
    {
        bool hasShownOptions = false;

        while (!hasShownOptions)
        {
            yield return new WaitForSeconds(0.1f);
            //Debug.Log("hasShownOptions:" + hasShownOptions + "   inLine:" + inLine);
            if (!inLine)
            {
                //Debug.Log("Showing options...");
                string speaker = "";
                foreach (SpeechLine line in options)
                {
                    if (!string.IsNullOrEmpty(line.speakerName))
                    {
                        speaker = line.speakerName;
                        break;
                    }
                }

                GameObject currDialogueUI = uiElements[speaker];
                DialogueBox dialogueBox = currDialogueUI.GetComponent<DialogueBox>();

                // put together a dialogue option line
                string optionsLineText = "";
                foreach (SpeechLine line in options)
                {
                    //optionsLineText += "<color=\"black\">" + line.optionNum + ": " + "<color=#7D7D7D>" + line.synopsisText;
                    optionsLineText += "> " + line.optionNum + ": " + line.synopsisText;
                    optionsLineText += "\n";
                }
                optionsLineText = "<color=#7D7D7D> " + optionsLineText;

                dialogueBox.currLine = optionsLineText;

                if (!currentDialogue.interactionEffects.Contains("STATIC"))
                {
                    // show this ui box (if following)
                    currDialogueUI.SetActive(true);
                }
                else
                {
                    // if static, inject it into the other character's box
                    uiElements[currentDialogue.lines[lineNum - 1][0].speakerName].GetComponent<DialogueBox>().currLine += "\n" + optionsLineText;
                }

                hasShownOptions = true;
            }
        }
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

    public static void ApplyDialogueEffects()
    {
        foreach (string str in currentDialogue.interactionEffects)
        {
            switch(str)
            {
                case "FREEZE_CHAR_ZOOM":
                    //zoom char
                    if (characters["RU"].GetComponentInChildren<DOFControl>() == null)
                    {
                        dofControl = GameObject.Instantiate(DialogueEventController.dofController, characters["RU"].transform);
                    }
                    else
                    {
                        dofControl = characters["RU"].GetComponentInChildren<DOFControl>().gameObject;
                    }
                    dofControl.GetComponent<DOFControl>().ToggleFocusCamera();

                    goto case "FREEZE_CHAR";
                case "FREEZE_CHAR": // TODO: MAKE THIS MORE GENERIC (CURRENTLY HARD-CODED FOR RU)
                    // freeze char
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canMove = false;

                    // turn player to look at what we're focusing on
                    characters["KIKI"].transform.LookAt(new Vector3(characters["RU"].transform.position.x, characters["KIKI"].transform.position.y, characters["RU"].transform.position.z));

                    break;
                default: break;
            }
        }
    }

    public static void RevertDialogueEffects()
    {
        if(inStaticDialogue)
        {
            inStaticDialogue = false;
            GameObject.Find("UICanvases").GetComponent<CanvasManager>().SetGamestateByCanvasName("DialogueCanvas");
        }
        foreach (string str in currentDialogue.interactionEffects)
        {
            switch (str)
            {
                case "FREEZE_CHAR_ZOOM": 
                    if(dofControl != null)
                        dofControl.GetComponent<DOFControl>().ToggleFocusCamera();

                    // delete the DOF controller
                    //Destroy(dofControl, 3f);

                    goto case "FREEZE_CHAR";
                case "FREEZE_CHAR":
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().canMove = true; // TODO: MAKE THIS MORE GENERIC (CURRENTLY HARD-CODED FOR RU)
                    break;
                default: break;
            }
        }
    }

    static void ClearPreviousEvent()
    {
        if(currentDialogue != null)
        {
            RevertDialogueEffects();

            isChoosing = false;
            currOptionNum = -1;

            foreach (GameObject elem in uiElements.Values)
            {
                //elem.SetActive(false);
                Destroy(elem);
            }
        }
    }


}
