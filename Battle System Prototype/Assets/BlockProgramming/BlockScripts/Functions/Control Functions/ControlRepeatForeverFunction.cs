using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRepeatForeverFunction : ControlFunction
{
    public const string NAME="Repeat Forever";
    public override string getName(){return NAME;}

    private int loggedPointer;

    public override int function(int pointer,ref List<Block> blockStack){
        loggedPointer=++pointer;
        return loggedPointer;
    }

    public int onRepeat(int pointer,ref List<Block> blockStack){
        return loggedPointer;
    }
}
