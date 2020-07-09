using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechLine
{
    

    public string speakerName;
    public string synopsisText = ""; //optional
    public string lineText;
    public LineEffect lineEffect;
    //public int fontModifier; // these are left in so they can be applied at runtime

    public bool isBranch;
    public int optionNum;

}

public enum LineEffect // specifies what effect should be applied to the text box
{
    NONE,
    SHAKE,
    SLOW,
    FAST
}