using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandler : MonoBehaviour
{

    BlockProgrammerScript blockProgrammerScript;
    private GameObject inputBlock;
    public BlockType inputType;
    public BlockType secondaryInputType;
    public int inputFieldNumber;
    public bool textAllowed;
    private InputField inputField;
    private Header header;
    private InputSpace inputSpace;
    private Outline outline;

    public void Awake(){
        blockProgrammerScript=GameObject.FindGameObjectWithTag("BlockProgrammer").GetComponent<BlockProgrammerScript>();
        inputField=GetComponent<InputField>();
        header=transform.parent.gameObject.GetComponent<Header>();
        outline=GetComponent<Outline>();
        if(!textAllowed){
            inputField.enabled=false;
            inputSpace=new InputSpace();
            inputSpace.setInputFieldHandler(this);
            inputSpace.setPosition((Vector2)gameObject.GetComponent<RectTransform>().position);
            blockProgrammerScript.addInputSpace(inputSpace);
            if(transform.parent.GetComponentInChildren<Block>()!=null){
                inputSpace.setActive(false);
                inputBlock=transform.parent.GetComponentInChildren<Block>().gameObject;
            }else{
                inputSpace.setActive(true);
            }
        }
    }   

    public Header getHeader(){return header;}

    public string getText(){return inputField.text;}

    public void setHighlight(bool highlight){
        outline.enabled=highlight;
    }

    public void setInputSpaceActive(bool active){
        if(!textAllowed&&inputBlock==null){
            inputSpace.setActive(active);
        }else if(inputBlock!=null){
            inputSpace.setActive(false);
        }
    }

    public void updateInputSpacePosition(){
        if(!textAllowed){
            inputSpace.setPosition((Vector2)gameObject.transform.position);
        }
    }

    public void addInputBlock(GameObject inputBlock){
        this.inputBlock=inputBlock;
        header.insertInputBlock(inputBlock,this);
        gameObject.SetActive(false);
        inputSpace.setActive(false);
    }

    public void clearInputBlock(){
        inputBlock=null;
        gameObject.SetActive(true);
        inputSpace.setActive(true);
    }

    public GameObject? getInputBlock(){return inputBlock;}

    public void removeInputSpace(){
        blockProgrammerScript.removeInputSpace(inputSpace);
    }
}
