using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum attribute
{
    Weight,
    Size,
    Friction,
    Location,
    empty
};

public enum discriptor
{
    Increases,
    Decreases,
    Frozen,
    empty
};

public enum type
{
    discriptor,
    attribute
}

public class Word
{

    public string word { get; set; }

    public type wordType;

    public Word()
    {
        word = "";
    }

    public Word(string Word)
    {
        word = Word;
    }



}
