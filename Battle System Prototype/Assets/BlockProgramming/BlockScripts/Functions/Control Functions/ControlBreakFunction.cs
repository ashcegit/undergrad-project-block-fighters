using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBreakFunction : ControlFunction
{
    private const string NAME="Break";

    public override string getName(){return NAME;}

    public override int function(int pointer, ref List<Block> blockStack)
    {
        while(blockStack[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()!="Repeat"
            ||blockStack[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()!="Repeat Until"
            ||blockStack[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()!="Repeat Forever"){
                pointer++;
        }
        return ++pointer;
    }
}
