using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventHandler : MonoBehaviour
{
    DialogueEventController dialogueEventController;
    // Start is called before the first frame update
    void Start()
    {
        //dialogueEventController = GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>();

        //StartFirstDialogue();
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
        yield return new WaitForSeconds(2f);
        dialogueEventController.ExecuteEvent("UNIQUE_NARRATIVE_OPENING");
    }

    public void PickedUpEvent(string itemName)
    {
        Debug.Log("PICKED UP ITEM NAME: "  + itemName);
        if(itemName == "Decrease")
        {
            dialogueEventController.ExecuteEvent("UNIQUE_PICKUP_INCREASEDECREASE");
        }
        if (itemName == "Size")
        {
            dialogueEventController.ExecuteEvent("UNIQUE_PICKUP_SIZE");
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
