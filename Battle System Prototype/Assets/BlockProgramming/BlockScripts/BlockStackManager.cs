using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The block stack is a linearisation of the blocks in a given method.
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

    public void initBlockStack(Block block){
        //performs a depth search for all blocks starting from a given parent block
        List<Section> sections=block.getSections();
        if(block.blockType!=BlockType.Method){
            blockStack.Add(block);
        }
        foreach(Section section in sections){
            if(section.getBody()!=null){
                foreach(Transform childTransform in section.getBody().gameObject.transform){
                    Block childBlock=childTransform.gameObject.GetComponent<Block>();
                    initBlockStack(childBlock);
                }
                Block endBlock=new Block();
                endBlock.blockType=BlockType.EndOfSection;
                blockStack.Add(endBlock);
            }            
        }
    }

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
                executionWrapper.setGameAction(currentBlock.gameObject.GetComponent<ActionFunction>().function(target,instigator));
                executionWrapper.setEndOfSection(false);
                break;
            case(BlockType.Control):
                pointer++;
                executionWrapper.setGameAction(null);
                executionWrapper.setEndOfSection(false);
                break;
            case(BlockType.Operator):
                pointer++;
                executionWrapper.setGameAction(null);
                executionWrapper.setEndOfSection(false);
                break;
            case(BlockType.Logic):
                pointer++;
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
