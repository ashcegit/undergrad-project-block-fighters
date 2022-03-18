using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlIfFunction : ControlFunction
{
    private const string NAME="If";

    public override string getName(){return NAME;}

    public override int function(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        bool result;
        pointer++;

        if(!logicFunction.function()){
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
            result = false;
        } else {
            result = true;
        }
        Terminal.log(TerminalLogType.Control, "If block {0}", result?"triggered":"not triggered");
        return pointer;
    }
}
