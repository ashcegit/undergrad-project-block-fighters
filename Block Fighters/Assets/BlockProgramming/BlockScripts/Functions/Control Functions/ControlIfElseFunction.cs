using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class ControlIfElseFunction : ControlFunction
{
    private const string NAME="If Else";
    public override string getName(){return NAME;}

    private bool triggered;

    public override int function(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        Block inputBlock=gameObject.GetComponent<Block>().getSections()[0].getHeader().getInputFieldHandlers()[0].getInputBlock().GetComponent<Block>();
        LogicFunction logicFunction=inputBlock.GetComponent<LogicFunction>();
        pointer++;
        if(logicFunction.function()){
            triggered=true;
            Terminal.log(TerminalLogType.Control, "If Else block triggered, 'If blocks' running", thisBlock.gameObject.name, triggered);
        } else {
            triggered=false;
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
            Terminal.log(TerminalLogType.Control, "If Else block not triggered, 'Else blocks' running", thisBlock.gameObject.name, triggered);
        }
        return pointer;
    }

    public int onRepeat(int pointer,ref List<Block> blockStack){
        Block thisBlock=gameObject.GetComponent<Block>();
        if(triggered){
            while(blockStack[pointer].getStartBlock()!=thisBlock){
                pointer++;
            }
            triggered=false;
        }
        return ++pointer;
    }
}
