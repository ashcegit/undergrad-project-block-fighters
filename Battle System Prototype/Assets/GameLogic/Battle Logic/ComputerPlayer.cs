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

    public void initBlockStack(int levelCounter){
        initComputerBlockStackLevel1A();
    }

    public void initComputerBlockStackLevel1A(){
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

public ExecutionWrapper executeCurrentBlock(){
        ExecutionWrapper executionWrapper=new ExecutionWrapper();
        if(pointer>=blockStack.Count){
            executionWrapper.setGameAction(null);
            executionWrapper.setEndOfSection(false);
        }else{
            Block currentBlock=blockStack[pointer];
            switch(currentBlock.blockType){
                case(BlockType.Action):
                    pointer++;
                    GameScript gameScript=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();
                    Character opponent=gameScript.getOpponent();
                    ActionFunction actionFunction=currentBlock.gameObject.GetComponent<ActionFunction>();
                    executionWrapper.setGameAction(actionFunction.function(opponent,true));
                    executionWrapper.setEndOfSection(false);
                    break;
                case(BlockType.Control):
                    ControlFunction controlFunction=currentBlock.gameObject.GetComponent<ControlFunction>();
                    pointer=controlFunction.function(pointer,ref blockStack);
                    executionWrapper.setGameAction(null);
                    executionWrapper.setEndOfSection(false);
                    break;
                case(BlockType.EndOfSection):
                    Block startBlock=currentBlock.getStartBlock();
                    if(startBlock!=null){
                        if(startBlock.getBlockType()==BlockType.Control){
                            ControlFunction endControlFunction=startBlock.gameObject.GetComponent<ControlFunction>();
                            switch(endControlFunction.getName()){
                                case("If Else"):
                                    ControlIfElseFunction controlIfElseFunction=(ControlIfElseFunction)endControlFunction;
                                    pointer=controlIfElseFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat"):
                                    ControlRepeatFunction controlRepeatFunction=(ControlRepeatFunction)endControlFunction;
                                    pointer=controlRepeatFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat Until"):
                                    ControlRepeatUntilFunction controlRepeatUntilFunction=(ControlRepeatUntilFunction)endControlFunction;
                                    pointer=controlRepeatUntilFunction.onRepeat(pointer,ref blockStack);
                                    break;
                                case("Repeat Forever"):
                                    ControlRepeatForeverFunction controlForeverFunction=(ControlRepeatForeverFunction)endControlFunction;
                                    pointer=controlForeverFunction.onRepeat(pointer,ref blockStack);
                                    break;
                            }
                        }
                    }
                    break;
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
