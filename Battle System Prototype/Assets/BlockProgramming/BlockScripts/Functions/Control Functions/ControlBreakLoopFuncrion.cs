using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBreakLoopFuncrion : ControlFunction
{
    private string NAME="Break Loop";

    public override string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockList){
        while(blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat"
            ||blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat Until"
            ||blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat Forever"){
                pointer++;
            }
        return ++pointer;
        
    }
}
