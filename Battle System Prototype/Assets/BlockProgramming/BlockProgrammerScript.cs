using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProgrammerScript : MonoBehaviour
{
    Vector2 lastPosition;
    Raycaster raycaster;
    GameObject environment;
    GhostBlock ghostBlock;
    List<GameObject> blockObjects;
    List<Transform> availableSpaces;
    List<GameObject> methodBlockObjects;
    int maxMethodBlocks;

    void Awake(){
        environment=GameObject.FindGameObjectWithTag("BlockEnvironment");
        maxMethodBlocks=4;
    }   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){

    }

    void OnDisable(){

    }

    void execute(){
        
    }

    public void addMethodBlock(GameObject methodBlockObject){
        methodBlockObjects.Add(methodBlockObject);
    }

    public void removeMethodBlock(GameObject methodBlockObject){
        methodBlockObjects.Remove(methodBlockObject);
    }

    public List<GameObject> getMethodBlockObjects(){return methodBlockObjects;}

    public int getMethodBlockObjectLength(){return methodBlockObjects.Count;}

    public void setMaxMethodBlocks(int maxMethodBlocks){this.maxMethodBlocks=maxMethodBlocks;}

    public int getMaxMethodBlocks(){return maxMethodBlocks;}

    public bool moreMethodBlocksAllowed(){return getMethodBlockObjectLength()<maxMethodBlocks;}

    
}
