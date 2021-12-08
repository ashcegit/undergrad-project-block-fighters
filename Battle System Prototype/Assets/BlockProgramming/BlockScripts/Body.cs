using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void addBlock(GameObject blockObject){
        blockObject.transform.SetParent(transform);
    }

    public void insertBlock(GameObject blockObject,int index){
        blockObject.transform.SetParent(transform);
        blockObject.transform.SetSiblingIndex(index);
    }

    public void removeBlock(GameObject blockObject){
        
    }

}
