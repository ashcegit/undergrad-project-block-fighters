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

    public void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        sections=new List<Section>();
        foreach(Transform child in transform){
            if(child.GetComponent<Section>()!=null){
                sections.Add(child.GetComponent<Section>());
            }
        }
        if(blockType==BlockType.Method){blockStackManager=new BlockStackManager();}
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
    }

    public void updateSpacePositions(){
        foreach(Section section in sections){
            section.updateSpacePositions();
        }
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
        blockStackManager.initBlockStack(this);
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
}
