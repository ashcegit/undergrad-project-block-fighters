using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    List<Section> sections;
    BlockProgrammerScript blockProgrammerScript;
    int pointer;

    public void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        sections=new List<Section>();
        foreach(Transform child in transform){
            if(child.GetComponent<Section>()!=null){
                sections.Add(child.GetComponent<Section>());
            }
        }
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

    public void updateBlockSpacePositions(){
        foreach(Section section in sections){
            section.updateBlockSpacePositions();
        }
    }

    public void setBlockSpacesActive(bool active){
        foreach(Section section in sections){
            section.blockSpacesActive(active);
        }
    }

    public void execute(){

    }

}
