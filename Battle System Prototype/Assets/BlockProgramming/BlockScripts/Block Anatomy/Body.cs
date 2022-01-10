using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    BlockProgrammerScript blockProgrammerScript;
    List<BlockSpace> blockSpaces;
    bool dragged;

    void Awake(){
        blockSpaces=new List<BlockSpace>();
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
    }

    // Start is called before the first frame update
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
        for(int i=index+1;i<blockSpaces.Count;i++){
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
        for(int i=1;i<transform.childCount;i++){
            blockSpaces[i].setPosition((Vector2)transform.GetChild(i).GetComponent<RectTransform>().position);
        }
        foreach(Transform childTransform in transform){
            if(childTransform.GetComponent<Block>()!=null){
                childTransform.GetComponent<Block>().updateSpacePositions();
            }
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
        RectTransform rectTransform=gameObject.GetComponent<RectTransform>();
        float width=0f;
        float height=35f;
        float heightDelta=0f;
        foreach(Transform childTransform in transform){
            Block childBlock=childTransform.gameObject.GetComponent<Block>();
            if(childBlock!=null){
                //Debug.Log(childBlock.updateBlockLayouts().y);
                height+=childBlock.updateBlockLayouts().y;
            }else{
                height+=childTransform.gameObject.GetComponent<RectTransform>().sizeDelta.y;
            }
        }
        width+=15f;

        if(width<150f){width=150f;}

        height+=heightDelta;
        if(height<120f){height=120f;}

        Vector2 sizeVector=new Vector2(width,height);
        rectTransform.sizeDelta=sizeVector;
        return sizeVector;
    }
}