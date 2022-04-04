using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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
    StaminaText staminaText;
    Regex methodNameRegex;

    void Awake(){
        methodBlockObjects=new List<GameObject>();
        blockObjects=new List<GameObject>();
        blockSpaces=new List<BlockSpace>();
        inputSpaces=new List<InputSpace>();
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
        maxMethodBlocks=4;
        blockSave=new BlockSave();
        staminaText = GetComponentInChildren<StaminaText>();
        methodNameRegex = new Regex(@"^[a-zA-Z][0-9a-zA-Z]*$");
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

    public void resetGame() {
        clearEnvironment();
        blockSave = new BlockSave();
        refreshBlockSelection();
    }

    public void clearEnvironment(){
        foreach(GameObject blockObject in blockObjects){
            Destroy(blockObject);
        }
        foreach(GameObject methodBlockObject in methodBlockObjects){
            Destroy(methodBlockObject);
        }
        blockObjects = new List<GameObject>();
        methodBlockObjects = new List<GameObject>();
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

    public bool areInputFieldHandlersEmpty() {
        bool flag = false;
        foreach(GameObject methodblockObject in methodBlockObjects) {
            if (methodblockObject.GetComponent<Block>().areInputFieldHandlersEmpty()) {
                flag = true;
            }
        }
        return flag;
    }

    public bool areMethodNamesLegal() {
        bool flag = true;
        foreach(GameObject methodBlockObject in methodBlockObjects) {
            Debug.Log(methodNameRegex.IsMatch(methodBlockObject.GetComponent<Block>().methodName));
            if (!methodNameRegex.IsMatch(methodBlockObject.GetComponent<Block>().methodName)) {
                flag = false;
            }
            Debug.Log(flag);
        }
        return flag;
    }

    public List<BlockSpace> getBlockSpaces(){return blockSpaces;}
    public List<InputSpace> getInputSpaces(){return inputSpaces;}

    public void applyMethodNames(){
        foreach (GameObject methodBlockObject in methodBlockObjects){
            methodBlockObject.GetComponent<Block>().setMethodNameFromHeader();
        }
    }

    public List<SelectionBlock> unlockRandomBlocks(){
        return blockSave.unlockRandomBlocks();
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
            if(panelTransform.gameObject.name=="Panel Info") {
                heightVector.y += 100f;
            } else {
                heightVector.y += 2f;
            }
            panelTransform.GetComponent<RectTransform>().sizeDelta=heightVector;
        }
    }

    public bool finishProgrammingCheck(){
        foreach(GameObject methodBlockObject in methodBlockObjects){
            if(methodBlockObject.GetComponent<Block>().getMethodName()==""){
                return false;
            }
        }
        return true;
    }

    public bool checkForNonEmptyMethods(){
        foreach(GameObject methodBlockObject in methodBlockObjects){
            if(methodBlockObject.GetComponentInChildren<Body>().transform.childCount==0){
                return false;
            }
        }
        return true;
    }

    public bool checkMaxAmountOfMethods(){
        if(methodBlockObjects.Count>5){
            return false;
        }else{
            return true;
        }
    }

    public bool checkMinAmountOfMethods(){
        if(methodBlockObjects.Count<1){
            return false;
        }else{
            return true;
        }
    }

    public void cleanUpStrayBlocks(){
        Transform environmentTransform=GameObject.FindGameObjectWithTag("BlockEnvironment").transform;
        foreach(Transform childTransform in environmentTransform){
            if(childTransform.GetComponent<Block>().getBlockType()!=BlockType.Method){
                Destroy(childTransform.gameObject);
            }
        }
    }

    public void displayStamina(int stamina) {
        staminaText.changeText(stamina.ToString());
    }
}
