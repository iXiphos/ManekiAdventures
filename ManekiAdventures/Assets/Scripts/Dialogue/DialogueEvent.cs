using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent
{
    // NOTE: SPEECHLINE VARIABLES ARE {{ }}, THIS CLASS SHOULD INJECT THE CORRECT VARIABLE FOR THE SITUATION

    
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
    
}
