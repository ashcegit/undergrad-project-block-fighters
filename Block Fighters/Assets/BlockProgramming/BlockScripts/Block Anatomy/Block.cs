using System.Collections;
using System.Collections.Generic;
using System;
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
        GetComponent<RectTransform>().position=position;
        updateSpacePositions();
    }

    public void updateSpacePositions(){
        foreach(Section section in sections){
            section.updateSpacePositions();
        }
    }

    public Vector2 updateBlockLayouts(){
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 initialSize = rectTransform.sizeDelta;
        Vector2 sizeSum=new Vector2();
        Vector2 sectionSizeDelta=new Vector2();
        foreach(Section section in sections){
            sectionSizeDelta=section.updateBlockLayouts();
            sizeSum.y+=sectionSizeDelta.y;
            if(sizeSum.x<sectionSizeDelta.x){sizeSum.x=sectionSizeDelta.x;}
        }
        GetComponent<RectTransform>().sizeDelta=sizeSum;
        //if (transform.parent.GetComponent<Block>() == null) {
        //    Debug.Log("Position: " + rectTransform.position);
        //    Debug.Log("Size Sum: " + sizeSum);
        //    Debug.Log("Initial Size: " + initialSize);
        //    rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y-sizeSum.y+initialSize.y);
        //}
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

    public Tuple<GameAction?,bool> executeCurrentBlock(){
        return blockStackManager.executeCurrentBlock();
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

    public bool areInputHandlersEmpty() {
        bool flag = false;
        foreach(Section section in sections) {
            if (section.areInputHandlersEmpty()) {
                flag = true;
            }
        }
        return flag;
    }
}
