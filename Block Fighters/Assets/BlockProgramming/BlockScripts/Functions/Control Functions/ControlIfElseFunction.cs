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
        if(logicFunction.function()){
            triggered=true;
            Terminal.log(TerminalLogType.Control, "If Else block triggered, 'If blocks' running", thisBlock.gameObject.name, triggered);
        } else {
            triggered=false;
            while (pointer < blockStack.Count-1) {
                Block currentBlock = blockStack[pointer];
                if (currentBlock.getBlockType() == BlockType.EndOfSection) {
                    if (currentBlock.getStartBlock() == thisBlock) {
                        Debug.Log("Start block found");
                        pointer++;
                        break;
                    }
                } else {
                    pointer++;
                }
            }
            Terminal.log(TerminalLogType.Control, "If Else block not triggered, 'Else blocks' running", thisBlock.gameObject.name, triggered);
        }
        return pointer;
    }

    public override int onRepeat(int pointer,ref List<Block> blockStack){
        //fix If/Else block stack
        Debug.Log("In onRepeat");   
        Block thisBlock=gameObject.GetComponent<Block>();
        if(triggered){
            Debug.Log("Code skipped, pre-skip pointer: " + pointer);
            while (pointer < blockStack.Count-1) {
                Block currentBlock = blockStack[pointer];
                if (currentBlock.getBlockType() == BlockType.EndOfSection) {
                    if (currentBlock.getStartBlock() == thisBlock) {
                        Debug.Log("Start block found");
                        break;
                    }
                } else {
                    pointer++;
                }
            }
            Debug.Log("Post-skip pointer: " + ++pointer);
            triggered=false;
        }
        return ++pointer;
    }
}
