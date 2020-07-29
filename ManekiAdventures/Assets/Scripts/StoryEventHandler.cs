using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventHandler : MonoBehaviour
{
    DialogueEventController dialogueEventController;
    public static Dictionary<string, bool> uniqueEventTracker; //string name of event, bool hasInteracted? (true if yes)

    // Start is called before the first frame update
    void Start()
    {
        uniqueEventTracker = new Dictionary<string, bool>();
        

        //StartFirstDialogue();
    }

    IEnumerator FindDialogueControllerAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();
    }

    public void StartFirstDialogue()
    {
        dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();

        // REALLY BASIC FOR NOW. JUST USED TO FIRE THE FIRST SCRIPT. ADD MORE LATER.
        StartCoroutine(FireFirstDialogue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireFirstDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueEventController.ExecuteEvent("UNIQUE_NARRATIVE_OPENING");
    }

    public void PickedUpEvent(string itemName)
    {
        string eventName = "";
        switch(itemName)
        {
            case "Decrease":
            case "Increase":
                eventName = "UNIQUE_PICKUP_INCREASEDECREASE";
                break;
            case "Size":
                eventName = "UNIQUE_PICKUP_SIZE";
                break;
            case "Weight":
                eventName = "UNIQUE_PICKUP_WEIGHT";
                break;
            case "Friction":
                eventName = "UNIQUE_PICKUP_FRICTION";
                break;
        }

        if (uniqueEventTracker.ContainsKey(eventName)) // if the player has seen this dialogue, do not execute
            return;
        else
            uniqueEventTracker[eventName] = true; // keep track of what you've seen


        dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();
        dialogueEventController.ExecuteEvent(eventName);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
