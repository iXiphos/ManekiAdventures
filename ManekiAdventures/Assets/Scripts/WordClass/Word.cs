using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds all the Attibutes, Discriptors
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
