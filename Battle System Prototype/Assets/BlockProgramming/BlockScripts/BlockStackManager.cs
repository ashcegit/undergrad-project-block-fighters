using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStackManager:MonoBehaviour
{
    private int pointer;
    private List<Block> blockStack;

    void Awake(){
        pointer=0;
        blockStack=new List<Block>();
    }

    public void initBlockStack(Block block){
        List<Section> sections=block.getSections();
        foreach(Section section in sections){
            if(section.getBody().gameObject.transform.childCount>0){
                foreach(Transform childTransform in section.getBody().gameObject.transform){
                    Block childBlock=childTransform.gameObject.GetComponent<Block>();
                    initBlockStack(childBlock);
                }
                if(block.blockType!=BlockType.Method){
                    blockStack.Add(block);
                }
            }
            Block endBlock=new Block();
            endBlock.blockType=BlockType.EndOfSection;
            blockStack.Add(endBlock);
        }
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

    public GameAction? executeCurrentBlock(Character target,Character instigator){
        Block currentBlock=blockStack[pointer];
        switch(currentBlock.blockType){
            case(BlockType.Action):
                pointer++;
                return currentBlock.gameObject.GetComponent<ActionFunction>().function(target,instigator);
                break;
            case(BlockType.Control):
                pointer++;
                return null;
                break;
            case(BlockType.Operator):
                pointer++;
                return null;
                break;
            case(BlockType.Logic):
                pointer++;
                return null;
                break;
            case(BlockType.EndOfSection):
            default:
                pointer++;
                return null;
                break;
        }
        
    }

}
