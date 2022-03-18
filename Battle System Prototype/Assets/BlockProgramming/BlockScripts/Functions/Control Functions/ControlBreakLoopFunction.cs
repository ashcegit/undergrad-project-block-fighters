using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlBreakLoopFunction : ControlFunction
{
    private string NAME="Break Loop";

    public override string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockList){
        bool foundFlag=false;
        while(!foundFlag){
            if(blockList[pointer].getStartBlock()!=null){
                if(blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()=="Repeat"||
                    blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()=="Repeat Until"||
                    blockList[pointer].getStartBlock().gameObject.GetComponent<ControlFunction>().getName()=="Repeat Forever"){
                        foundFlag=true;
                    }
            }else{
                pointer++;
            }
        }
        Terminal.log(TerminalLogType.Message, "Break block triggered");
        return ++pointer;
        
    }
}
