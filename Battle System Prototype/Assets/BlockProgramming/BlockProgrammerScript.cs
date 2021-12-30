using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProgrammerScript : MonoBehaviour
{
    Vector2 lastPosition;
    Raycaster raycaster;
    GameObject environment;
    List<GameObject> methodBlockObjects;
    List<GameObject> blockObjects;
    List<BlockSpace> blockSpaces;
    int maxMethodBlocks;

    void Awake(){
        methodBlockObjects=new List<GameObject>();
        blockObjects=new List<GameObject>();
        blockSpaces=new List<BlockSpace>();
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
        maxMethodBlocks=4;
    }   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        foreach(Transform childTransform in transform){
            childTransform.gameObject.SetActive(true);
        }
    }

    void OnDisable(){
        foreach(Transform childTransform in transform){
            childTransform.gameObject.SetActive(false);
        }
    }


    public void addBlockObject(GameObject blockObject){
        if(blockObject.GetComponent<Block>().getBlockType()==BlockType.Method){
            methodBlockObjects.Add(blockObject);
        }else{
            blockObjects.Add(blockObject);
        }
    }

    public void removeBlockObject(GameObject blockObject){
        if(blockObject.GetComponent<Block>().getBlockType()==BlockType.Method){
            methodBlockObjects.Remove(blockObject);
        }else{
            blockObjects.Remove(blockObject);
        }
        Destroy(blockObject);
    }

    public void clearEnvironment(){
        foreach(GameObject blockObject in blockObjects){
            Destroy(blockObject);
        }
        foreach(GameObject methodBlockObject in methodBlockObjects){
            Destroy(methodBlockObject);
        }
    }

    public void addBlockSpace(BlockSpace blockSpace){
        blockSpaces.Add(blockSpace);
    }

    public void removeBlockSpace(BlockSpace blockSpace){
        blockSpaces.Remove(blockSpace);
    }

    public List<GameObject> getMethodBlockObjects(){return methodBlockObjects;}

    public int getMethodBlockObjectLength(){return methodBlockObjects.Count;}

    public void setMaxMethodBlocks(int maxMethodBlocks){this.maxMethodBlocks=maxMethodBlocks;}

    public int getMaxMethodBlocks(){return maxMethodBlocks;}

    public bool moreMethodBlocksAllowed(){return getMethodBlockObjectLength()<maxMethodBlocks;}

    public List<BlockSpace> getBlockSpaces(){return blockSpaces;}    
}
