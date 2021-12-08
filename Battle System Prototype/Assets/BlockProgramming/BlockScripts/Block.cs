using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    Transform parent;
    List<GameObject> children;
    List<Section> sections;
    List<GameObject> bodies;
    public BlockType blockType;
    BlockProgrammerScript blockProgrammerScript;

    public void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
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
}
