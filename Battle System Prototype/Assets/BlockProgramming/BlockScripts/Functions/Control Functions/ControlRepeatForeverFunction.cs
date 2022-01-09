using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRepeatForeverFunction : ControlFunction
{
    public const string NAME="Repeat Forever";
    public string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockStack){
        return 0;
    }
}
