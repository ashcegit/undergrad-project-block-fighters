using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBreakLoopFuncrion : ControlFunction
{
    public string name="Break Loop";
    public override int function(int pointer,ref List<Block> blockList){
        while(blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat"
            ||blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat Until"
            ||blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().name!="Repeat Forever"){
                pointer++;
            }
        return ++pointer;
        
    }
}
