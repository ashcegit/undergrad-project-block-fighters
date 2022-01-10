using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    private List<InputFieldHandler> inputFieldHandlers;

    void Awake(){
        inputFieldHandlers=new List<InputFieldHandler>();
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandlers.Add(inputFieldHandler);
            }
        }
    }

    public void addInputFieldHandler(InputFieldHandler inputFieldHandler){
        inputFieldHandlers.Add(inputFieldHandler);
    }

    public List<string> getInputStrings(){
        List<string> inputStrings=new List<string>();
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
            inputStrings.Add(inputFieldHandler.getText());
        }
        return inputStrings;
    }

    public void insertInputBlock(GameObject inputBlock,InputFieldHandler inputFieldHandler){
        int index=inputFieldHandler.transform.GetSiblingIndex();
        inputBlock.transform.SetParent(transform);
        inputBlock.transform.SetSiblingIndex(index+1);
    }

    public void setInputSpacesActive(bool active){
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandler.setInputSpaceActive(active);
            }
        }
    }

    public void updateInputSpacePositions(){
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandler.updateInputSpacePosition();
            }
        }
    }

    public InputFieldHandler getInputFieldHandlerForInputBlock(GameObject inputBlock){
        InputFieldHandler rtn=null;
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
            if(inputFieldHandler.getInputBlock()==inputBlock){
                rtn=inputFieldHandler;
            }
        }
        return rtn;
    }

    public List<InputFieldHandler> getInputFieldHandlers(){return inputFieldHandlers;}

    public void initActionInputFieldHandler(){
        inputFieldHandlers[0].initActionInputFieldHandler();
    }

    public Vector2 updateBlockLayouts(){
        Block thisBlock=GetComponentInParent<Block>();
        if(thisBlock!=null){
            if(thisBlock.getBlockType()==BlockType.Method){
                return gameObject.GetComponent<RectTransform>().sizeDelta;
            }
        }
        RectTransform rectTransform=gameObject.GetComponent<RectTransform>();
        float width=0f;
        float height=60f;
        float heightDelta=0f;
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers){
            GameObject inputBlock=inputFieldHandler.getInputBlock();
            if(inputBlock!=null){
                Vector2 inputBlockSize=inputBlock.GetComponent<Block>().getSections()[0].getHeader().updateBlockLayouts();
                width+=inputBlockSize.x;
                if(inputBlockSize.y>heightDelta){heightDelta=inputBlockSize.y;}
            }
        }
        width+=15f;

        if(width<150f){width=150f;}

        height+=heightDelta;
        if(height<60f){height=60f;}

        Vector2 sizeVector=new Vector2(width,height);
        rectTransform.sizeDelta=sizeVector;
        return sizeVector;
    }
}
