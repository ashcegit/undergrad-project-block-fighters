using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    private List<Section> sections;
    private BlockProgrammerScript blockProgrammerScript;
    public string name;
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

    public string getName(){return name;}
    public void setName(string name){this.name=name;}

    public List<Section> getSections(){return sections;}

    public void initBlockStack(){
        blockStackManager.initBlockStack(this);
    }

    public GameAction? executeCurrentBlock(Character target,Character instigator){
        return blockStackManager.executeCurrentBlock(target,instigator);
    }
}
