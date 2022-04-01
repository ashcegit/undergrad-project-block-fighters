using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlBreakLoopFunction : ControlFunction
{
    private string NAME="Break Loop";

    public override string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockStack){
        bool foundFlag=false;
        while(!foundFlag){
            Block currentBlock = blockStack[pointer];
            if (currentBlock.getBlockType() == BlockType.EndOfSection) {
                if (currentBlock.getStartBlock() != null) {
                    if (currentBlock.getStartBlock().gameObject.GetComponent<ControlFunction>().getName() == "Repeat" ||
                        currentBlock.getStartBlock().gameObject.GetComponent<ControlFunction>().getName() == "Repeat Until" ||
                        currentBlock.getStartBlock().gameObject.GetComponent<ControlFunction>().getName() == "Repeat Forever") {
                        foundFlag = true;
                    }
                } else {
                    pointer++;
                }
            }
        }
        Terminal.log(TerminalLogType.Message, "Break block triggered");
        return ++pointer;   
    }

    public override int onRepeat(int pointer, ref List<Block> blockStack) {
        return ++pointer;
    }
}
