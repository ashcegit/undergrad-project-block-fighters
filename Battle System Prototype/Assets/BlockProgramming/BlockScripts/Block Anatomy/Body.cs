using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    BlockProgrammerScript blockProgrammerScript;
    List<BlockSpace> blockSpaces;
    Vector2 initialSizeDelta;
    bool dragged;

    void Awake(){
        blockSpaces=new List<BlockSpace>();
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        initialSizeDelta=GetComponent<RectTransform>().sizeDelta;
    }

    void Start()
    {
        BlockSpace startingBlockSpace=new BlockSpace();
        startingBlockSpace.setParentBody(gameObject);
        startingBlockSpace.setPosition((Vector2)GetComponentInParent<Section>().gameObject
                                                .GetComponentInChildren<Header>().gameObject.GetComponent<RectTransform>().position);
        startingBlockSpace.setActive(false);
        startingBlockSpace.setIndex(0);
        blockSpaces.Add(startingBlockSpace);
        blockProgrammerScript.addBlockSpace(startingBlockSpace);
    }

    public void insertBlock(GameObject blockObject,int index){
        blockObject.transform.SetParent(transform);
        blockObject.transform.SetSiblingIndex(index);
        BlockSpace newBlockSpace=new BlockSpace();
        newBlockSpace.setParentBody(gameObject);
        newBlockSpace.setActive(true);
        blockSpaces.Add(newBlockSpace);
        blockProgrammerScript.addBlockSpace(newBlockSpace);
        for(int i=0;i<blockSpaces.Count;i++){
            blockSpaces[i].setIndex(i);
        }
    }

    public void removeBlockSpaceByIndex(int index){
        if(blockSpaces.Count>1){
            BlockSpace blockSpace=blockSpaces[index];
            blockSpaces.Remove(blockSpace);
            blockProgrammerScript.removeBlockSpace(blockSpace);
            for(int i=0;i<blockSpaces.Count;i++){
                blockSpaces[i].setIndex(i);
            }
        }
    }

    public void updateSpacePositions(){
        blockSpaces[0].setPosition((Vector2)GetComponentInParent<Section>().gameObject
                                            .GetComponentInChildren<Header>().gameObject.GetComponent<RectTransform>().position);
        for(int i=0;i<transform.childCount;i++){
            blockSpaces[i].setPosition((Vector2)transform.GetChild(i).GetComponent<RectTransform>().position);
            transform.GetChild(i).GetComponent<Block>().updateSpacePositions();
        }
    }

    public void setBlockSpacesActive(bool active){
        foreach(BlockSpace blockSpace in blockSpaces){
            blockSpace.setActive(active);
            foreach(Transform childTransform in transform){
                Block childBlock=childTransform.gameObject.GetComponent<Block>();
                if(childBlock!=null){
                    childBlock.setSpacesActive(active);
                }
            }
        }
    }

    public Vector2 updateBlockLayouts(){
        RectTransform rectTransform=GetComponent<RectTransform>();
        float width=initialSizeDelta.x;
        float height;
        if(GetComponentInParent<Block>().getBlockType()==BlockType.Method||
            transform.parent.parent.childCount>2&&
            transform.parent.GetSiblingIndex()<transform.parent.parent.childCount-2){
            height=0f;
        }else{
            height=40f;
        }
        Vector2 deltaVector;
        Vector2 sizeVector=new Vector2();
        if(transform.childCount>0){
            foreach(Transform childTransform in transform){
                Block childBlock=childTransform.gameObject.GetComponent<Block>();
                if(childBlock.name=="GhostBlock"){
                    height+=90f;
                }else if(childBlock!=null){
                    deltaVector=childBlock.updateBlockLayouts();
                    height+=deltaVector.y;
                    if(width<deltaVector.x){
                        width=deltaVector.x;
                    } 
                }
            }
            height-=10f*(float)(transform.childCount);
            //width+=20f;
        }else{
            width=initialSizeDelta.x;
            height=initialSizeDelta.y; 
        }
        sizeVector=new Vector2(width,height);
        rectTransform.sizeDelta=sizeVector;
        return sizeVector;
    }
}