using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    private List<Section> sections;
    private BlockProgrammerScript blockProgrammerScript;
    public string methodName;
    private BlockStackManager blockStackManager;
    private Block startBlock;

    public void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        sections=new List<Section>();
        foreach(Transform child in transform){
            if(child.GetComponent<Section>()!=null){
                sections.Add(child.GetComponent<Section>());
            }
        }
        if(blockType==BlockType.Method){blockStackManager=new BlockStackManager();}
        startBlock=null;
    }

    void OnEnable(){
    
    }

    void OnDisable(){
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public BlockType getBlockType(){return blockType;}

    public void setPosition(Vector2 position){
        transform.position=position;
        updateSpacePositions();
    }

    public void updateSpacePositions(){
        foreach(Section section in sections){
            section.updateSpacePositions();
        }
    }

    public Vector2 updateBlockLayouts(){
        Vector2 sizeSum=new Vector2();
        foreach(Section section in sections){
            sizeSum+=section.updateBlockLayouts();
        }
        return sizeSum;
    }

    public void setSpacesActive(bool active){
        foreach(Section section in sections){
            section.setSpacesActive(active);
        }
    }

    public string getMethodName(){return methodName;}
    public void setMethodNameFromHeader(){
        methodName=sections[0].getHeader().getInputStrings()[0];
    }

    public List<Section> getSections(){return sections;}

    public void initBlockStack(){
        blockStackManager.setBlockStack(blockStackManager.initBlockStack(this));
    }

    public ExecutionWrapper executeCurrentBlock(Character target,Character instigator){
        return blockStackManager.executeCurrentBlock(target,instigator);
    }

    public void clearBlockStack(){
        blockStackManager.clearBlockStack();
    }

    public int getBlockStackCount(){return blockStackManager.getBlockStackCount();}

    public void removeInputSpaces(){
        foreach(Section section in sections){
            section.removeInputSpaces();
        }
    }

    public void setStartBlock(Block startBlock){
        this.startBlock=startBlock;
    }

    public Block getStartBlock(){
        return startBlock;
    }

    public void initActionInputFieldHandler(){
        sections[0].initActionInputFieldHandler();
    }
}
