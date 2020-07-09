using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueText
{
    /* Contains all lines of text and effects associated with each line.
     *   Creates DialogueText data from a text document.
     */

    string fileName;
    private string[] rawTextArray; // raw lines from text file (not processed)


    public List<string> interactionEffects = new List<string>();
    public List<List<SpeechLine>> lines = new List<List<SpeechLine>>(); // contains all lines of dialogue in order
                            // if it reads in an array of longer than 1, 
                            //then it assumes multiple text options and 
                            //expects the next line to have an array of 
                            //the same size (if not then it will complain)

    public void ReadRawLinesFromFile() // if no file name is provided, use stored filename 
    {
        ReadRawLinesFromFile(fileName);
    }

    public void ReadRawLinesFromFile(string file)
    {
        string rawTxt = Resources.Load<TextAsset>("Dialogue\\" + file).text; // attempt to retreive dialogue text
        
        rawTextArray = rawTxt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        ProcessLines();
    }

    void ProcessLines()
    {
        List<SpeechLine> currentLines = new List<SpeechLine>();

        foreach (string line in rawTextArray)
        {
            if (string.IsNullOrEmpty(line)) { continue; } // skip line if it's empty
            
            string processedLine = line;
            if (line.IndexOf("//") > -1) { processedLine = line.Substring(0, line.IndexOf("//")); } // remove comments

            bool isSpeakingLine = true;
            string speaker = "";

            if (processedLine.IndexOf(':') > -1) // determine speaker OR effect
            {
                string preText = processedLine.Substring(0, processedLine.IndexOf(':'));
                processedLine = processedLine.Remove(0, processedLine.IndexOf(':') + 1); // remove pretext from the line
                preText = preText.Replace(":", string.Empty);

                

                // process preText
                if (!string.IsNullOrEmpty(preText))
                {
                    // if this line has pretext, save and clear the previous line(s)
                    if (currentLines.Count > 0) { lines.Add(currentLines); } // add prev if it had anything
                    currentLines = new List<SpeechLine>(); // clear and make a new instance

                    switch (preText) // check if pretext is a tag
                    {
                        case "EFFECTS": // if this is an interaction effect, add it to the list
                            string[] effectTags = processedLine.Split(' ');
                            foreach (string str in effectTags) { interactionEffects.Add(str); }
                            isSpeakingLine = false;
                            break;

                        // add more cases as needed

                        default: // default: if it's not an effect, then this is a name
                            speaker = preText;
                            isSpeakingLine = true;
                            break;
                    }
                }
            }

            if (!isSpeakingLine || string.IsNullOrEmpty(processedLine)) { continue; }
            else //continue processing if this is a speaker line
            {
                int optionNum = -1; // defaults to -1 if no branching
                string lineText = "";
                string synopsisText = "";
                LineEffect lineEffect = (LineEffect)0;
                bool isBranching = false;

                // check if this is a branching line
                if (processedLine.Contains("~"))
                {
                    isBranching = true;
                    string[] branches = processedLine.Split('~');
                    foreach (string option in branches)
                    {
                        if(!string.IsNullOrEmpty(option))
                        {
                            // get option number (and remove it from the rest of the string)
                            optionNum = Int32.Parse(option.Substring(0, option.IndexOf(" ")));
                            lineText = option.Remove(0, option.IndexOf(" ")); // remove the number in the beginning

                            // process the synopsis text (and remove it)
                            if(lineText.IndexOf('%') != -1)
                            {
                                synopsisText = lineText.Substring(lineText.IndexOf('%'), lineText.LastIndexOf('%') - lineText.IndexOf('%'));
                                lineText = lineText.Remove(lineText.IndexOf('%'), lineText.LastIndexOf('%') - lineText.IndexOf('%') + 1);

                            }

                            // process lines
                            processedLine = ProcessSpeech(lineText, ref lineEffect);
                        }
                    }
                }
                else //non-branching: process normally
                {
                    // process lines
                    processedLine = ProcessSpeech(processedLine, ref lineEffect);
                }

                // after processing is complete, make a SpeechLine & add it
                SpeechLine lineToAdd = new SpeechLine()
                {
                    speakerName = speaker,
                    synopsisText = synopsisText,
                    lineText = processedLine,
                    lineEffect = lineEffect,
                    isBranch = isBranching,
                    optionNum = optionNum
                };

                currentLines.Add(lineToAdd);
            }
        }
    }
    
    string ProcessSpeech(string lineText, ref LineEffect lineEffect)
    {
        // process and format TMP font sizes
        while (lineText.IndexOf("<<") > 0) // keep processing until there are no more increases
        {
            int indexOfLastBracket = lineText.IndexOf(">>");
            while ((indexOfLastBracket + 1) < lineText.Length && lineText[indexOfLastBracket + 1]  == '>')
            {
                indexOfLastBracket++;
            }

            string stringToReplace = lineText.Substring(lineText.IndexOf("<<"), indexOfLastBracket - lineText.IndexOf("<<") + 1);
            string stringToFormat = stringToReplace;
            int timesToInc = stringToFormat.Split('<').Length; // counts how many brackets: if there is none, this should be 1(?)
            stringToFormat = stringToFormat.Replace("<", string.Empty).Replace(">", string.Empty); //clean the string of << and >> characters

            stringToFormat = "<size=" + (100 + (10 * timesToInc)).ToString() + "%>" + stringToFormat + "<size=100%>";
            lineText = lineText.Replace(stringToReplace, stringToFormat);
        }

        while (lineText.IndexOf("[[") > 0) // keep processing until there are no more decreases
        {
            int indexOfLastBracket = lineText.IndexOf("]]");
            while ((indexOfLastBracket + 1) < lineText.Length && lineText[indexOfLastBracket + 1] == ']')
            {
                indexOfLastBracket++;
            }

            string stringToReplace = lineText.Substring(lineText.IndexOf("[["), indexOfLastBracket - lineText.IndexOf("[[") + 1);
            string stringToFormat = stringToReplace;
            int timesToInc = stringToFormat.Split('[').Length; // counts how many brackets: if there is none, this should be 1(?)
            stringToFormat = stringToFormat.Replace("[", string.Empty).Replace("]", string.Empty); //clean the string of << and >> characters

            stringToFormat = "<size=" + (100 - (10 * timesToInc)).ToString() + "%>" + stringToFormat + "<size=100%>";
            lineText = lineText.Replace(stringToReplace, stringToFormat);
        }

        // process any line effects
        if(lineText.IndexOf('[') > -1)
        {
            string lineEffectStr = lineText.Substring(lineText.IndexOf('['), lineText.IndexOf(']') - lineText.IndexOf('['));
            lineEffect = stringToLineEffect(lineEffectStr);
            lineText = lineText.Remove(lineText.IndexOf('['), lineText.IndexOf(']') - lineText.IndexOf('[') + 1);
        }

        // remove extra whitespace
        lineText = lineText.Trim(' ');

        return lineText;
    }

    LineEffect stringToLineEffect(string str)
    {
        switch(str)
        {
            case "SHAKE": return LineEffect.SHAKE;
            case "SLOW": return LineEffect.SLOW;
            case "FAST": return LineEffect.FAST;
            default: return LineEffect.NONE;
        }
    }
}
