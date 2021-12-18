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
                                                .GetComponentInChildren<Header>().gameObject.transform.position);
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
        BlockSpace blockSpace=blockSpaces[index+1];
        blockSpaces.Remove(blockSpace);
        blockProgrammerScript.removeBlockSpace(blockSpace);
        for(int i=index;i<blockSpaces.Count;i++){
            blockSpaces[i].setIndex(i);
        }
    }

    public void updateBlockSpacePositions(){
        Debug.Log("test");
        blockSpaces[0].setPosition((Vector2)GetComponentInParent<Section>().gameObject
                                            .GetComponentInChildren<Header>().gameObject.transform.position);
        for(int i=1;i<transform.childCount;i++){
            blockSpaces[i].setPosition((Vector2)transform.GetChild(i).transform.position);
        }
        foreach(Transform child in transform){
            if(child.GetComponent<Block>()!=null){
                child.GetComponent<Block>().updateBlockSpacePositions();
            }
        }
    }

    public void blockSpacesActive(bool active){
        foreach(BlockSpace blockSpace in blockSpaces){
            blockSpace.setActive(active);
        }
    }
}