using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The block stack is a linearisation of all attack/control blocks in a given method.
//By adding endOfSection blocks, I can linearlly serach over the player's code
//while controlling where in the code is being executed via an internal pointer variable
public class BlockStackManager
{
    private int pointer;
    private List<Block> blockStack;

    public BlockStackManager(){
        pointer=0;
        blockStack=new List<Block>();
    }

    public List<Block> initBlockStack(Block block){
        //performs a depth search for all non-header blocks starting from a given parent block
        List<Block> blocks=new List<Block>();
        List<Section> sections=block.getSections();
        if(block.blockType!=BlockType.Method){
            blocks.Add(block);
        }
        foreach(Section section in sections){
            if(section.getBody()!=null){
                foreach(Transform childTransform in section.getBody().gameObject.transform){
                    Block childBlock=childTransform.gameObject.GetComponent<Block>();
                    blocks.AddRange(initBlockStack(childBlock));
                }
                Block endBlock=new Block();
                endBlock.blockType=BlockType.EndOfSection;
                endBlock.setStartBlock(block);
                blocks.Add(endBlock);
            }            
        }
        return blocks;
    }

    public void setBlockStack(List<Block> blockStack){
        this.blockStack=blockStack;
    }

    public List<Block> getBlockStack(){return blockStack;}

    public void clearBlockStack(){
        pointer=0;
        blockStack=new List<Block>();
    }

    public GameAction? executeCurrentBlock() {
        GameAction gameAction = null;
        if (pointer >= blockStack.Count) {
            executionWrapper.setGameAction(null);
        } else {
            Block currentBlock = blockStack[pointer];
            switch (currentBlock.blockType) {
                case (BlockType.Action):
                    pointer++;
                    GameScript gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
                    Character opponent = gameScript.getOpponent();
                    ActionFunction actionFunction = currentBlock.gameObject.GetComponent<ActionFunction>();
                    gameAction = actionFunction.function(opponent, true);
                    break;
                case (BlockType.Control):
                    ControlFunction controlFunction = currentBlock.gameObject.GetComponent<ControlFunction>();
                    pointer = controlFunction.function(pointer, ref blockStack);
                    break;
                case (BlockType.EndOfSection):
                    Block startBlock = currentBlock.getStartBlock();
                    if (startBlock != null) {
                        if (startBlock.getBlockType() == BlockType.Control) {
                            ControlFunction endControlFunction = startBlock.gameObject.GetComponent<ControlFunction>();
                            switch (endControlFunction.getName()) {
                                case ("If Else"):
                                    ControlIfElseFunction controlIfElseFunction = (ControlIfElseFunction)endControlFunction;
                                    pointer = controlIfElseFunction.onRepeat(pointer, ref blockStack);
                                    break;
                                case ("Repeat"):
                                    ControlRepeatFunction controlRepeatFunction = (ControlRepeatFunction)endControlFunction;
                                    pointer = controlRepeatFunction.onRepeat(pointer, ref blockStack);
                                    break;
                                case ("Repeat Until"):
                                    ControlRepeatUntilFunction controlRepeatUntilFunction = (ControlRepeatUntilFunction)endControlFunction;
                                    pointer = controlRepeatUntilFunction.onRepeat(pointer, ref blockStack);
                                    break;
                                case ("Repeat Forever"):
                                    ControlRepeatForeverFunction controlForeverFunction = (ControlRepeatForeverFunction)endControlFunction;
                                    pointer = controlForeverFunction.onRepeat(pointer, ref blockStack);
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    pointer++;
                    break;
            }
        }
        return gameAction;
    }

    public int getBlockStackCount(){return blockStack.Count;}

}
