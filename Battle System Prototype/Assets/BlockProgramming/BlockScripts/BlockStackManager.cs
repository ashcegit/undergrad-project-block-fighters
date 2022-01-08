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
        blockStack=new List<Block>();
        List<Section> sections=block.getSections();
        if(block.blockType!=BlockType.Method){
            blockStack.Add(block);
        }
        foreach(Section section in sections){
            if(section.getBody()!=null){
                foreach(Transform childTransform in section.getBody().gameObject.transform){
                    Block childBlock=childTransform.gameObject.GetComponent<Block>();
                    Debug.Log("Adding range");
                    blockStack.AddRange(initBlockStack(childBlock));
                }
                Block endBlock=new Block();
                endBlock.blockType=BlockType.EndOfSection;
                endBlock.setStartBlock(block);
                blockStack.Add(endBlock);
            }            
        }
        Debug.Log(blockStack);
        return blockStack;
    }

    public void setBlockStack(List<Block> blockStack){
        this.blockStack=blockStack;
    }

    public List<Block> getBlockStack(){return blockStack;}

    public void clearBlockStack(){
        pointer=0;
        blockStack=new List<Block>();
    }

    public void insertBlockStack(List<Block> newBlockStack){
        for(int i=0;i<newBlockStack.Count;i++){
            blockStack.Insert(i+pointer,newBlockStack[i]);
        }
    }

    public void removeFromBlockStackAtIndex(int index){
        blockStack.RemoveAt(index);
    }

    public void removeBlocksUntilNextEndOfSection(){
        for(int i=pointer;i<blockStack.Count;i++){
            Block block=blockStack[i];
            if(block.blockType==BlockType.EndOfSection){
                break;
            }else{
                blockStack.Remove(block);
            }
        }
    }

    public ExecutionWrapper executeCurrentBlock(Character target,Character instigator){
        ExecutionWrapper executionWrapper=new ExecutionWrapper();
        if(pointer>=blockStack.Count){
            executionWrapper.setGameAction(null);
            executionWrapper.setEndOfSection(false);
        }else{
            Block currentBlock=blockStack[pointer];
            switch(currentBlock.blockType){
            case(BlockType.Action):
                pointer++;
                ActionFunction actionFunction=currentBlock.gameObject.GetComponent<ActionFunction>();
                executionWrapper.setGameAction(actionFunction.function(null,instigator));
                executionWrapper.setEndOfSection(false);
                break;
            case(BlockType.Control):
                ControlFunction controlFunction=currentBlock.gameObject.GetComponent<ControlFunction>();
                pointer=controlFunction.function(pointer,blockStack);
                executionWrapper.setGameAction(null);
                executionWrapper.setEndOfSection(false);
                break;
            case(BlockType.EndOfSection):
            default:
                pointer++;
                executionWrapper.setGameAction(null);
                executionWrapper.setEndOfSection(true);
                break;
            }
        }
        return executionWrapper;
    }

    public int getBlockStackCount(){return blockStack.Count;}

}
