using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ComputerPlayer:MonoBehaviour
{
    private GameObject punchPrefab;
    private GameObject kickPrefab;
    private List<Block> blockStack;
    private int pointer;

    public ComputerPlayer(){
        pointer=0;
        blockStack=new List<Block>();
        punchPrefab=(GameObject)Resources.Load("Prefabs/Blocks/Actions/Attacks/Block Action Attack Punch");
        kickPrefab=(GameObject)Resources.Load("Prefabs/Blocks/Actions/Attacks/Block Action Attack Kick");
    }

    public void initComputerBlockStackA(){
        for(int i=0;i<5;i++){
            blockStack.Add(Instantiate(punchPrefab).GetComponent<Block>());
        }
        Block endBlock=new Block();
        endBlock.blockType=BlockType.EndOfSection;
        blockStack.Add(endBlock);
    }

    public void clearBlockStack(){
        foreach(Block block in blockStack){
            if(block.blockType!=BlockType.EndOfSection){
                Destroy(block.gameObject);
            }
        }
        blockStack=new List<Block>();
        pointer=0;
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
