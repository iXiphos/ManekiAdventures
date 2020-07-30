using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockUniqueInteraction : UniquePotionInteraction
{
    public override void ExecuteUniqueInteraction()
    {
        if (StoryEventHandler.uniqueEventTracker.ContainsKey("UNIQUE_THROWPOTION")) // if the player has seen this dialogue, do not execute
            return;
        else
            StoryEventHandler.uniqueEventTracker["UNIQUE_THROWPOTION"] = true;

        // fire event
        GameObject.Find("DialogueEventController").GetComponent<DialogueEventController>().ExecuteEvent("UNIQUE_THROWPOTION");
    }
}
