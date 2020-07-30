using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTrigger : MonoBehaviour
{
    public string eventFileName;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(eventFileName.Contains("UNIQUE"))
            {
                if (StoryEventHandler.uniqueEventTracker.ContainsKey(eventFileName)) // if the player has seen this dialogue, do not execute
                    return;
                else
                    StoryEventHandler.uniqueEventTracker[eventFileName] = true; // keep track of what you've seen
            }

            GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>().ExecuteEvent(eventFileName);
        }
    }
}
