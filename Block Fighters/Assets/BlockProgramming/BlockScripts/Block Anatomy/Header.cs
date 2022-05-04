using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Header : MonoBehaviour
{
    private List<InputFieldHandler> inputFieldHandlers;
    private Vector2 initialSizeDelta;

    void Awake(){
        RectTransform rectTransform=GetComponent<RectTransform>();
        initialSizeDelta=rectTransform.sizeDelta;
        inputFieldHandlers=new List<InputFieldHandler>();
        foreach(Transform childTransform in transform){
            InputFieldHandler inputFieldHandler=childTransform.gameObject.GetComponent<InputFieldHandler>();
            if(inputFieldHandler!=null){
                inputFieldHandlers.Add(inputFieldHandler);
                inputFieldHandler.Awake();
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

    public bool areInputFieldHandlersEmpty() {
        bool flag = false;
        foreach(InputFieldHandler inputFieldHandler in inputFieldHandlers) {
            if (inputFieldHandler.getInputBlock() == null&&!inputFieldHandler.textAllowed) {
                flag = true;
            }
        }
        return flag;
    }

    public Vector2 updateBlockLayouts(){
        RectTransform rectTransform=gameObject.GetComponent<RectTransform>();
        Vector2 sizeVector;
        if(transform.parent.transform.GetSiblingIndex()==0){
            Block thisBlock=GetComponentInParent<Block>();
            if(thisBlock!=null){
                if(thisBlock.getBlockType()==BlockType.Method){
                    return gameObject.GetComponent<RectTransform>().sizeDelta;
                }
            }
            float width=25f;
            float height=10f;
            float heightDelta=0f;
            foreach (Transform headerChild in transform) {
                if (headerChild.gameObject.GetComponent<Block>() != null) {
                    Vector2 blockSize = headerChild.GetComponent<Block>().getSections()[0].getHeader().updateBlockLayouts();
                    width += blockSize.x;
                    if (blockSize.y > heightDelta) {
                        heightDelta = initialSizeDelta.y;
                    }
                    width += 10f;
                } else if (headerChild.gameObject.GetComponent<InputFieldHandler>() != null) {
                    if (headerChild.gameObject.active) {
                        headerChild.gameObject.GetComponent<InputFieldHandler>().updateInputSpacePosition();
                        width += headerChild.GetComponent<RectTransform>().sizeDelta.x;
                    }
                } else {
                    width += headerChild.GetComponent<RectTransform>().sizeDelta.x;
                    width += 10f;
                }
            }
            height += heightDelta;
            if (heightDelta < initialSizeDelta.y) {
                height = initialSizeDelta.y;
            }
            if (width < initialSizeDelta.x) {
                width = initialSizeDelta.x;
            }
            sizeVector=new Vector2(width,height);
            rectTransform.sizeDelta=sizeVector;
        }else{
            sizeVector=new Vector2(rectTransform.sizeDelta.x,rectTransform.sizeDelta.y);
        }
        
        return sizeVector;
    }
}
