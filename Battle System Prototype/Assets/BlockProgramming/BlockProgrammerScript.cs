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
    List<InputSpace> inputSpaces;
    int maxMethodBlocks;
    BlockSave blockSave;

    void Awake(){
        methodBlockObjects=new List<GameObject>();
        blockObjects=new List<GameObject>();
        blockSpaces=new List<BlockSpace>();
        inputSpaces=new List<InputSpace>();
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
        maxMethodBlocks=4;
        blockSave=new BlockSave();
        refreshBlockSelection();
    }   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        foreach(Transform childTransform in transform){
            childTransform.gameObject.SetActive(true);
        }
        refreshBlockSelection();
    }

    void OnDisable(){
        foreach(Transform childTransform in transform){
            childTransform.gameObject.SetActive(false);
        }
    }


    public void addBlockObject(GameObject blockObject){
        if(blockObject.GetComponent<Block>().getBlockType()==BlockType.Method){
            if(!methodBlockObjects.Contains(blockObject)){
                methodBlockObjects.Add(blockObject);
            }
        }else{
            if(!blockObjects.Contains(blockObject)){
                blockObjects.Add(blockObject);
            }
        }
    }

    public void removeBlockObject(GameObject blockObject){
        if(blockObject.GetComponent<Block>().getBlockType()==BlockType.Method){
            methodBlockObjects.Remove(blockObject);
        }else{
            blockObjects.Remove(blockObject);
        }
        blockObject.GetComponent<Block>().removeInputSpaces();
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

    public void addInputSpace(InputSpace inputSpace){
        inputSpaces.Add(inputSpace);
    }

    public void removeBlockSpace(BlockSpace blockSpace){
        blockSpaces.Remove(blockSpace);
    }

    public void removeInputSpace(InputSpace inputSpace){
        inputSpaces.Remove(inputSpace);
    }

    public List<GameObject> getMethodBlockObjects(){return methodBlockObjects;}

    public int getMethodBlockObjectLength(){return methodBlockObjects.Count;}

    public void setMaxMethodBlocks(int maxMethodBlocks){this.maxMethodBlocks=maxMethodBlocks;}

    public int getMaxMethodBlocks(){return maxMethodBlocks;}

    public bool moreMethodBlocksAllowed(){return getMethodBlockObjectLength()<maxMethodBlocks;}

    public List<BlockSpace> getBlockSpaces(){return blockSpaces;}
    public List<InputSpace> getInputSpaces(){return inputSpaces;}

    public void applyMethodNames(){
        foreach(GameObject methodBlockObject in methodBlockObjects){
            methodBlockObject.GetComponent<Block>().setMethodNameFromHeader();
        }
    }

    public void unlockRandomBlock(){
        blockSave.unlockRandomBlock();
    }

    public void refreshBlockSelection(){
        blockSave.refreshSelectionBlocks();
        GameObject selectionContent=GameObject.FindGameObjectWithTag("SelectionContent");
        foreach(Transform panelTransform in selectionContent.transform){
            RectTransform rectTransform=panelTransform.GetComponent<RectTransform>();
            Vector2 heightVector=new Vector2();
            foreach(Transform blockTransform in panelTransform.transform){
                if(blockTransform.gameObject.activeSelf){
                    heightVector.y+=blockTransform.GetComponent<RectTransform>().sizeDelta.y;
                }
                heightVector.y+=5f;
            }
            heightVector.x=rectTransform.sizeDelta.x;
            heightVector.y+=2f;
            panelTransform.GetComponent<RectTransform>().sizeDelta=heightVector;
        }
    }
}
