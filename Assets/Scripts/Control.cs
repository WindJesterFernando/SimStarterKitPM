using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System;


public abstract class ControlBase
{
    public int controlKey;
    public int controlKeyState;

    public ControlBase(int controlKey, int controlKeyState)
    {
        this.controlKey = controlKey;
        this.controlKeyState = controlKeyState;
    }

    // public abstract int GetChildType()
    // {

    // }

    //serialization stuff
}

public class Control : ControlBase
{

    public Control(int controlKey, int controlKeyState)
        : base(controlKey, controlKeyState)
    {

    }

}

// public class InterpretedControl : ControlBase
// {

// }




public static class ControlKey
{
    public const int keyI = 1;
    public const int keyO = 2;
    public const int keyP = 3;

}

public static class ControlKeyState
{
    public const int Press = 1;
    public const int Release = 2;
    
    public const int Tap = 3;
    public const int DoubleTap = 4;
}



