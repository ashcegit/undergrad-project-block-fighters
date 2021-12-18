using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    BlockProgrammerScript blockProgrammerScript;
    GameObject buttonObject;
    Button button;

    void Awake(){
        buttonObject=gameObject;
        button=gameObject.GetComponent<Button>();
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
    }

    void Start()
    {
        button.onClick.AddListener(blockProgrammerScript.clearEnvironment);
    }

}
